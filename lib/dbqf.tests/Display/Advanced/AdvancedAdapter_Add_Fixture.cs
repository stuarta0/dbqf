using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.tests.Display.Advanced
{
    [TestFixture]
    public class AdvancedAdapter_Add_Fixture
    {
        /// <summary>
        /// Add a part with no existing parts.
        /// Outcome: part added and selected.
        /// </summary>
        [Test]
        public void No_parts()
        {

        }

        /// <summary>
        /// Add a part with an existing part that is selected.
        /// Outcome: Junction created, existing part and new part added to it, new part selected.
        /// </summary>
        [Test]
        public void One_part_selected()
        {

        }

        /// <summary>
        /// Add a part with an existing part that is not selected.
        /// Outcome: Junction created, existing part and new part added to it, new part selected. 
        /// </summary>
        [Test]
        public void One_part_not_selected()
        {

        }

        /// <summary>
        /// Add part with 2 parts in 1 junction, same junction type, 1 selected.
        /// Outcome: New part added to existing junction, selected.
        /// </summary>
        [Test]
        public void Two_in1_same_junction_selected()
        {

        }

        /// <summary>
        /// Add part with 2 parts in 1 junction, different junction type, 1 selected.
        /// Outcome: Junction created, selected part and new part added to this junction and this junction takes the place of selected part in existing junction with new part selected.
        /// </summary>
        [Test]
        public void Two_in1_different_junction_selected()
        {

        }

        /// <summary>
        /// Add part with 2 parts in 1 junction, same junction type, no selection.
        /// Outcome: New part added to existing junction, selected.
        /// </summary>
        [Test]
        public void Two_in1_same_junction_no_selection()
        {

        }

        /// <summary>
        /// Add part with 2 parts in 1 junction, different junction type, no selection.
        /// Outcome: Junction created, existing junction and new part added to new junction which also becomes the root junction, new part selected.
        /// </summary>
        [Test]
        public void Two_in1_different_junction_no_selection()
        {

        }
    }
}
