using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Core
{
    /// <summary>
    /// Lớp IDataGridView
    /// </summary>
    public class IDataGridView : DataGridView
    {
        public IDataGridView() : base()
        {
            DataBindingComplete += IDataGridView_DataBindingComplete;
            RowsAdded += IDataGridView_RowsAdded;
        }

        [DefaultValue(true)]
        public virtual bool ShowCheckBox { get; set; }

        CheckBox headerCheckBox = new CheckBox();
        private void AddCheckBox()
        {
            //Add a CheckBox Column to the DataGridView Header Cell.

            //Find the Location of Header Cell.
            Point headerCellLocation = GetCellDisplayRectangle(0, -1, true).Location;

            //Place the Header CheckBox in the Location of the Header Cell.
            headerCheckBox.Location = new Point(Columns[0].Width / 2 - 5, 11);
            headerCheckBox.BackColor = Color.White;
            headerCheckBox.Size = new Size(18, 18);

            //Assign Click event to the Header CheckBox.
            headerCheckBox.Click += HeaderCheckBox_Clicked;
            Controls.Add(headerCheckBox);

            //Assign Click event to the DataGridView Cell.
            CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);
        }

        private void IDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (ShowCheckBox)
            {
                AddCheckBox();
            }
        }

        private void IDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (ShowCheckBox)
            {
                AddCheckBox();
            }
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells[0] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                headerCheckBox.Checked = isChecked;
            }
        }

        public bool IsCancelEdit { get; set; }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            IsCancelEdit = keyData == Keys.Escape;
            // bấm esc và đang ở chế độ editmode
            if (IsCurrentCellInEditMode && keyData == Keys.Return)
            {
                if (IsCancelEdit) base.CancelEdit();
                return base.EndEdit();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// bắt sự kiện nhấn vào các phím dùng để định hướng
        /// </summary>
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                base.EndEdit();
                return base.ProcessEscapeKey(e.KeyData);
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                if (IsCurrentCellInEditMode)
                    return false;
            }
            return base.ProcessDataGridViewKey(e);
        }
        /// <summary>
        /// bắt sự kiện nhấn vào các phím như TAB, ESCAPE, ENTER, và ARROW
        /// </summary>
        /// <param name="keyData">phím được nhập</param>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            //Keys keys = keyData & Keys.KeyCode;
            if (keyData == Keys.Oemplus)
            {
                EndEdit();
                return ProcessEscapeKey(keyData);
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}