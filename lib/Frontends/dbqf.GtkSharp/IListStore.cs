// IListModel.cs - A custom TreeModel for ILists 
// http://mono.1490590.n4.nabble.com/Re-MonoDevelop-feedback-and-suggestions-for-GTK-td1546804.html
//
// Author:  Christian Hoff  <[hidden email]> 
// 
// Copyright (c) 2009 Christian Hoff 
// 
// This program is free software; you can redistribute it and/or 
// modify it under the terms of version 2 of the Lesser GNU General 
// Public License as published by the Free Software Foundation. 
// 
// This program is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
// Lesser General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the 
// Free Software Foundation, Inc., 59 Temple Place - Suite 330, 
// Boston, MA 02111-1307, USA. 

using System; 
using System.Collections; 
using System.Collections.Generic; 
using System.ComponentModel; 
using System.Reflection; 

namespace Gtk { 
	internal class IListStore : GLib.Object, Gtk.TreeModelImplementor { 
		interface IListBindingHelper { 
			object DataSource { 
				get; 
			} 

			int NColumns { 
				get; 
			} 
			Type GetColumnType (int column); 

			int NItems { 
				get; 
			} 
			object GetValue (int idx, int column); 
		} 

		class ListBindingHelper<element_type> : IListBindingHelper { 
			object list; 
			bool is_generic_list; 
			PropertyDescriptorCollection prop_collection; 

			public ListBindingHelper (object list) 
			{ 
				this.list = list; 
				is_generic_list = list is IList<element_type>; 

				if (list is ITypedList) 
					prop_collection = (list as ITypedList).GetItemProperties (null); 
			} 

			public object DataSource { 
				get { 
					return list; 
				} 
			} 

			public int NColumns { 
				get { 
					return prop_collection == null ? 1 : prop_collection.Count; 
				} 
			} 

			Type GetNullableType (System.Type original_type) 
			{ 
				if (original_type.IsByRef || original_type == typeof (string)) 
					return original_type; 
				else 
					return typeof (Nullable<>).MakeGenericType (original_type);	
			} 

			public Type GetColumnType (int column) 
			{ 
				return prop_collection == null ? typeof (element_type) : GetNullableType (prop_collection [column].PropertyType); 
			} 

			public int NItems { 
				get { 
					return is_generic_list ? (list as IList<element_type>).Count : (list as IList).Count; 
				} 
			} 

			object GetItem (int idx) 
			{ 
				return is_generic_list ? (list as IList<element_type>) [idx] : (list as IList) [idx]; 
			} 

			public object GetValue (int idx, int column) 
			{ 
				object member = GetItem (idx); 
				if (prop_collection == null) 
					return member; 
				else { 
					object val = prop_collection [column].GetValue (member); 
					if (val == DBNull.Value) 
						return null; 
					else 
						return val; 
				} 
			} 
		} 

		IListBindingHelper binding_helper; 

		Gtk.TreeModelAdapter adapter; 
		int stamp; 
		// Save the number of items. Needed to emulate ListChangedType.Reset 
		int count = -1; 

		// We cannot take an IList since we also have to accept the generic IList<element_type> type. 
		public IListStore (object list) 
		{ 
			if (list == null) 
				throw new ArgumentNullException ("list"); 

			Type item_type = GetListItemType (list); 
			if (item_type == typeof (object)) 
				throw new NotSupportedException ("Lists of type object are not allowed. If you can narrow down the type of the items, the list should implement the generic IList interface or expose a public strongly typed Item property"); 

			binding_helper = Activator.CreateInstance (typeof (ListBindingHelper<>).MakeGenericType (item_type), new object[] {list}) as IListBindingHelper; 

			if (list is IBindingList) { 
				count = binding_helper.NItems; 
				(list as IBindingList).ListChanged += IBindingList_ListChanged; 
			} 

			// Create a random stamp for the iterators 
			Random random_stamp_gen = new Random (); 
			stamp = random_stamp_gen.Next (int.MinValue, int.MaxValue); 

			adapter = new Gtk.TreeModelAdapter (this); 
		} 

		static Type GetListItemType (object list) 
		{ 
			foreach (Type iface in list.GetType ().GetInterfaces ()) 
				if (iface.IsGenericType && iface.GetGenericTypeDefinition () == typeof (IList<>)) 
					return iface.GetGenericArguments () [0]; 

			if (list is IList) { 
				PropertyInfo item_property = GetItemProperty (list.GetType ()); 
				if (item_property == null) // `Item' could be interface-explicit, and thus private 
					return typeof (object); 
				else 
					return item_property.PropertyType; 
			} else 
				throw new ArgumentException ("Type {0} does not implement IList or IList<>", list.GetType ().FullName); 
		} 

		static PropertyInfo GetItemProperty (Type type) 
		{ 
			foreach (PropertyInfo prop in type.GetProperties (BindingFlags.Public | BindingFlags.Instance)) 
				if (prop.Name == "Item") 
					return prop; 

			return null; 
		} 

		public object DataSource { 
			get { 
				return binding_helper.DataSource; 
			} 
		} 

		public Gtk.TreeModelAdapter Adapter { 
			get { 
				return adapter; 
			} 
		} 

		public Gtk.TreeModelFlags Flags { 
			get { 
				return Gtk.TreeModelFlags.ListOnly; 
			} 
		} 

		#region Gtk.TreeIter handling 
		public Gtk.TreeIter GetIter (int idx) 
		{ 
			Gtk.TreeIter result = Gtk.TreeIter.Zero; 
			GetIter (idx, ref result); 

			return result; 
		} 

		private void GetIter (int idx, ref Gtk.TreeIter iter) 
		{ 
			// We can't pack pointers to the elements into the iters as IList allows duplicates; using the index instead 
			iter.UserData = new IntPtr (idx); 
			iter.Stamp = stamp; 
		} 

		public int GetIndex (Gtk.TreeIter iter) 
		{ 
			if (iter.Stamp != stamp) 
				throw new InvalidOperationException (String.Format ("iter belongs to a different model; it's stamp is not equal to the stamp of this model({0})", stamp)); 

			return iter.UserData.ToInt32 (); 
		} 

		public int IterNChildren (Gtk.TreeIter iter) 
		{ 
			if (iter.Equals (Gtk.TreeIter.Zero)) 
				return binding_helper.NItems; 
			else 
				return 0; 
		} 

		public bool IterHasChild (Gtk.TreeIter iter) 
		{ 
			return IterNChildren (iter) != 0; 
		} 

		public bool IterNthChild (out Gtk.TreeIter child, Gtk.TreeIter parent, int index) 
		{ 
			if (parent.Equals (Gtk.TreeIter.Zero) && binding_helper.NItems > 0) { 
				child = GetIter (index); 
				return true; 
			} else { 
				child = Gtk.TreeIter.Zero; 
				return false; 
			} 
		} 

		public bool IterChildren (out Gtk.TreeIter child, Gtk.TreeIter parent) 
		{ 
			return IterNthChild (out child, parent, 0); 
		} 

		public bool GetIterFirst (out Gtk.TreeIter iter) 
		{ 
			return IterNthChild (out iter, Gtk.TreeIter.Zero, 0); 
		} 

		public bool IterNext (ref Gtk.TreeIter iter) 
		{ 
			int new_index = GetIndex (iter) + 1; 
			if (new_index >= binding_helper.NItems) 
				return false; 

			GetIter (new_index, ref iter); 
			return true; 
		} 

		public bool IterParent (out Gtk.TreeIter parent, Gtk.TreeIter child) 
		{ 
			// List-only model 
			parent = Gtk.TreeIter.Zero; 
			return false; 
		} 
		#endregion 

		#region TreePath handling 
		public Gtk.TreePath GetPath (Gtk.TreeIter iter) 
		{ 
			return new Gtk.TreePath (new int[] { GetIndex (iter) }); 
		} 

		public bool GetIter (out Gtk.TreeIter iter, Gtk.TreePath path) 
		{ 
			iter = Gtk.TreeIter.Zero; 

			if (path.Indices.Length != 1) 
				return false; 

			int index = path.Indices [0]; 
			if (index >= binding_helper.NItems) 
				return false; 

			GetIter (index, ref iter); 
			return true; 
		} 
		#endregion 

		public void RefNode (Gtk.TreeIter iter) 
		{ 
		} 

		public void UnrefNode (Gtk.TreeIter iter) 
		{ 
		} 

		#region get/set model data 
		public int NColumns { 
			get { 
				return binding_helper.NColumns; 
			} 
		} 


		public GLib.GType GetColumnType (int column) 
		{ 
			return (GLib.GType) binding_helper.GetColumnType (column); 
		} 

		public void GetValue (Gtk.TreeIter iter, int column, ref GLib.Value val) 
		{ 
			// Console.WriteLine ("Getting value for column {0}, type should be {1}, value is of type: {2}", column, GetColumnSystemType (column).FullName, "bla"); 
			val.Init (GetColumnType (column)); 
			val.Val = GetValue (iter, column); 
		} 

		object GetValue (Gtk.TreeIter iter, int column) 
		{ 
			return binding_helper.GetValue (GetIndex (iter), column); 
		} 
		#endregion 

		#region IBindingList support 
		void IBindingList_ListChanged (object sender, ListChangedEventArgs args) 
		{ 
			if (args.ListChangedType != ListChangedType.Reset && args.NewIndex == -1 && args.OldIndex == -1) 
				return; 

			switch (args.ListChangedType) { 
			case ListChangedType.ItemAdded: 
				adapter.EmitRowInserted (new Gtk.TreePath (new int [] {args.NewIndex}), GetIter (args.NewIndex)); 
				break; 
			case ListChangedType.ItemDeleted: 
				adapter.EmitRowDeleted (new Gtk.TreePath (new int [] {args.NewIndex})); 
				break; 
			case ListChangedType.ItemChanged: 
				adapter.EmitRowChanged (new Gtk.TreePath (new int [] {args.NewIndex}), GetIter (args.NewIndex)); 
				break; 
			case ListChangedType.ItemMoved: 
				// Index: New position 
				// Value: Old position 
				int[] new_order = new int [binding_helper.NItems]; 
				for (int idx = 0; idx < binding_helper.NItems; idx++) { 
					if (idx == args.NewIndex) 
						new_order [idx] = args.OldIndex; 
					else if (idx == args.OldIndex) 
						new_order [idx] = args.NewIndex; 
					else 
						new_order [idx] = idx; 
				} 
				adapter.EmitRowsReordered (null, Gtk.TreeIter.Zero, new_order); 
				break; 
			case ListChangedType.Reset: 
				int n_items = binding_helper.NItems; 
				// "count" hasn't been refreshed yet, it is used to calculate how many items were removed or added to the list 
				int n_old = count; 

				if (n_items > n_old) { 
					for (int idx = n_old; idx < n_items; idx++) 
						adapter.EmitRowInserted (new Gtk.TreePath (new int [] {idx}), GetIter (idx)); 
				} else if (n_items < n_old) { 
					for (int idx = n_old - 1; idx >= n_items; idx--) 
						adapter.EmitRowDeleted (new Gtk.TreePath (new int [] {idx})); 
				} 

				int n_changed = Math.Min (n_items, n_old); 
				for (int idx = 0; idx < n_changed; idx++) 
					adapter.EmitRowChanged (new Gtk.TreePath (new int [] {idx}), GetIter (idx)); 
				break; 
			} 

			count = binding_helper.NItems; 
		} 
		#endregion 
	} 
} 