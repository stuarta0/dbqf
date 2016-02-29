using System;
using dbqf.Display;
using Gtk;

namespace dbqf.GtkSharp
{
	public class EntryElement : UIElement<Gtk.Widget>
	{
		private Entry _text;
		public EntryElement()
		{
			Entry = new Entry();
		}

		public virtual Entry Entry
		{
			get { return _text; }
			set 
			{
				if (_text != null)
				{
					_text.Changed -= OnTextChanged;
                    _text.Activated -= OnActivated;
				}

				_text = value;
				Element = _text;
				if (_text != null)
				{
					_text.Changed += OnTextChanged;
                    _text.Activated += OnActivated;
				}
			}
		}

        void OnTextChanged(object sender, EventArgs e)
		{
			OnChanged();
		}

        private void OnActivated(object sender, EventArgs e)
        {
            OnSearch();
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

