using System;
using dbqf.Display;
using Gtk;

namespace dbqf.GtkSharp
{
	public class EntryElement : UIElement<Gtk.Widget>
	{
		private Entry _text;
		public virtual Entry Entry
		{
			get { return _text; }
			set 
			{
				if (_text != null)
				{
					_text.Changed -= OnTextChanged;
					_text.KeyPressEvent -= OnKeyPress;
					//_text.TextChanged -= OnTextChanged;
					//_text.KeyDown -= OnKeyDown;
				}

				_text = value;
				Element = _text;
				if (_text != null)
				{
					_text.Changed += OnTextChanged;
					_text.KeyPressEvent += OnKeyPress;
				}
			}
		}

		void OnTextChanged(object sender, EventArgs e)
		{
			OnChanged();
		}

		void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			//if (e.Event == enter)
			//	OnSearch();
		}

		public EntryElement()
		{
			Entry = new Entry();
		}

		public override object[] GetValues()
		{
			if (!String.IsNullOrEmpty(Entry.Text))
				return new object[] { Entry.Text };
			return null;
		}

		public override void SetValues(params object[] values)
		{
			if (values != null && values.Length >= 1 && values[0] != null)
				Entry.Text = values[0].ToString();
			else
				Entry.Text = null;
		}
	}
}

