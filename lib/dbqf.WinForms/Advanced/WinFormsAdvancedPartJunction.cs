using dbqf.Display.Advanced;

namespace dbqf.WinForms.Advanced
{
    public class WinFormsAdvancedPartJunction : AdvancedPartJunction
    {
        public WinFormsAdvancedPartJunction()
            : base()
        { }
        public WinFormsAdvancedPartJunction(JunctionType type)
            : base(type)
        { }

        public void Delete()
        {
            OnDeleteRequested();
        }
    }
}
