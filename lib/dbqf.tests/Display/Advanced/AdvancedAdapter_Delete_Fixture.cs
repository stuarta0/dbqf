using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.tests.Display.Advanced
{
    [TestFixture]
    public class AdvancedAdapter_Delete_Fixture
    {
        /// <summary>
        /// A, no selection, delete A.
        /// Outcome: A is deleted.
        /// </summary>
        [Test]
        public void A_no_selection()
        {

        }

        /// <summary>
        /// A, selected, delete A.
        /// Outcome: Part is deleted and SelectedPart null
        /// </summary>
        [Test]
        public void A_selected()
        {

        }

        /// <summary>
        /// Delete part with 3 parts in 1 junction, not selected
        /// Outcome: Part is deleted, junction remains with 2 parts
        /// </summary>
        [Test]
        public void ABC_junction_no_selection_delete_A()
        {

        }

        /// <summary>
        /// Delete selected part with 3 parts in 1 junction, 1 selected.
        /// Outcome: Part is deleted, junction remains with 2 parts, SelectedPart null.
        /// </summary>
        [Test]
        public void ABC_junction_A_selected_delete_A()
        {

        }

        /// <summary>
        /// Delete root junction with 2 parts, no selection.
        /// Outcome: 2 parts and junction deleted, root null.
        /// </summary>
        [Test]
        public void AB_junction_no_selection_delete_junction()
        {

        }

        /// <summary>
        /// Delete root junction with 2 parts, 1 selected.
        /// Outcome: 2 parts and junction deleted, root null, SelectedPart null.
        /// </summary>
        [Test]
        public void AB_junction_A_selected_delete_junction()
        {

        }

        /// <summary>
        /// Delete part with 2 parts in 1 junction, no selection.
        /// Outcome: Part and junction deleted, remaining part becomes root
        /// </summary>
        [Test]
        public void AB_junction_no_selection_delete_A()
        {

        }

        /// <summary>
        /// Delete selected part with 2 parts in 1 junction, 1 selected.
        /// Outcome: Part and junction deleted, remaining part becomes root, SelectedPart null.
        /// </summary>
        [Test]
        public void AB_junction_A_selected_delete_A()
        {

        }

        /// <summary>
        /// and(A, B, or(C, D)), delete C
        /// Outcome: and(A, B, D).
        /// </summary>
        [Test]
        public void AB_orCD_junction_no_selection_delete_C()
        {

        }

        /// <summary>
        /// and(A, or(B, and(C, or(D, E selected)))), delete or(B, ...).
        /// Outcome: A, SelectedPart null
        /// </summary>
        [Test]
        public void A_orB_andC_orDE_junction_E_selected_delete_orB()
        {

        }
    }
}
