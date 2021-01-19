using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Compx323Project
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            Hide();
        }

        /// <summary>
        /// Base method to show the form, can be overriden to add more functionality.
        /// </summary>
        public virtual void ShowForm()
        {
            FormHelper.mainScreen.Hide();
            Show();
        }

        /// <summary>
        /// Sets the x button and button close events up.
        /// </summary>
        protected void SetupButtons()
        {
            FormClosing += FormExit;
            (Controls.Find("btnCancel", false)[0] as Button).Click += FormExit;
        }

        /// <summary>
        /// If the x button or the cancel button is pressed, the form will clear, hide, and show the main form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FormExit(object sender, EventArgs e)
        {
            if (e != null && e is FormClosingEventArgs)(e as FormClosingEventArgs).Cancel = true;
            foreach (Control c in Controls) c.Enabled = true;
            ClearInput();
            Hide();
            if (this is ModifiableForm) (this as ModifiableForm).updating = false;
            if (SQLHelper.lastQuery != "" && SQLHelper.lastTableName != "")
                SQLHelper.ExecuteDisplayQuery(SQLHelper.lastQuery, SQLHelper.lastTableName);
            FormHelper.mainScreen.Show();
        }

        /// <summary>
        ///  Clear all of the textbox controls text content in the form.
        /// </summary>
        protected void ClearInput()
        {
            foreach (Control c in Controls)
            {
                if (c is TextBox) (c as TextBox).Clear();
                else if (c is RichTextBox) (c as RichTextBox).Clear();
            }
        }

        public bool FillDropDown(string query, string error, ref List<string> ids, ref ComboBox dropDown)
        {
            DataTable table = SQLHelper.GetDataTable(query);
            List<string> dropDownItems = SQLHelper.GetDropDownList(table);
            if (dropDownItems == null)
            {
                MessageBox.Show(error);
                FormHelper.mainScreen.Show();
                Hide();
                return false;
            }
            dropDown.DataSource = dropDownItems;
            if (ids == null) return true;
            ids.Clear();
            foreach (DataRow row in table.Rows) ids.Add(row[1].ToString());
            return true;
        }

        public bool FillDropDown(string collection, ref List<string> ids, ref ComboBox dropDown)
        {
            List<string> dropDownItems = new List<string>();
            IMongoCollection<BsonDocument> dbCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>(collection);
            FilterDefinition<BsonDocument> all = Builders<BsonDocument>.Filter.Empty;
            var items = dbCollection.Find(all).ToListAsync().Result;
            string[] itemInfo;

            if (collection == "Caregiver")
            {
                foreach (var item in items)
                {
                    itemInfo = item.ToString().Split(',');
                    dropDownItems.Add(itemInfo[2].Split(':')[1].Replace("\"", "").Trim() + " " +
                        itemInfo[3].Split(':')[1].Replace("\"", "").Trim() + " from " +
                        itemInfo[6].Split(':')[1].Replace("\"", "").Replace("}", "").Trim());
                }
            }
            else if(collection == "Company")
            {
                foreach(var item in items)
                {
                    itemInfo = item.ToString().Split(',');
                    dropDownItems.Add(itemInfo[1].Split(':')[1].Replace("\"", "").Trim());
                }
            }




            if (dropDownItems.Count == 0)
            {
                MessageBox.Show("Failed to populate dropdown");
                FormHelper.mainScreen.Show();
                Hide();
                return false;
            }
            dropDown.DataSource = dropDownItems;
            if (ids == null) return true;
            ids.Clear();


            if (collection == "Caregiver")
            {
                foreach (var item in items)
                {
                    ids.Add(item.ToString().Split(',')[1].Split(':')[1].Replace("\"", "").Trim());
                }
            }
            else if(collection == "Company")
            {
                foreach(var item in items)
                {
                    ids.Add(item.ToString().Split(',')[1].Split(':')[1].Replace("\"", "").Trim());
                }
            }

            return true;
        }

        public bool FillMultiDropDown(string collection, ref List<string> ids1, ref List<string> ids2, ref ComboBox firstDropDown,ref ComboBox secondDropDown)
        {
            List<string> firstDropDownItems = new List<string>();
            List<string> secondDropDownItems = new List<string>();
            IMongoCollection<BsonDocument> firstCollection = null;
            IMongoCollection<BsonDocument> secondCollection = null;
            if (collection == "HasDisability")
            {
                firstCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Client");
                secondCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Disability");
            }
            else if (collection == "UsesAid")
            {
                firstCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Client");
                secondCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Aids");
            }
            FilterDefinition<BsonDocument> all = Builders<BsonDocument>.Filter.Empty;

            var itemSet1 = firstCollection.Find(all).ToListAsync().Result;
            var itemSet2 = secondCollection.Find(all).ToListAsync().Result;
            string[] itemInfo1;
            string[] itemInfo2;

            if(collection == "HasDisability")
            {
                
                
                foreach(var item in itemSet1)
                {
                    itemInfo1 = item.ToString().Split(',');
                    firstDropDownItems.Add(itemInfo1[2].Split(':')[1].Replace("\"", "").Trim() + " " +
                        itemInfo1[3].Split(':')[1].Replace("\"", "").Trim());
                }
                foreach (var item in itemSet2)
                {
                    itemInfo2 = item.ToString().Split(',');
                    secondDropDownItems.Add(itemInfo2[2].Split(':')[1].Replace("\"", "").Trim());
                }

            }
            else if(collection == "UsesAid")
            {
                foreach (var item in itemSet1)
                {
                    itemInfo1 = item.ToString().Split(',');
                    firstDropDownItems.Add(itemInfo1[2].Split(':')[1].Replace("\"", "").Trim() + " " +
                        itemInfo1[3].Split(':')[1].Replace("\"", "").Trim());
                }
                foreach (var item in itemSet2)
                {
                    itemInfo2 = item.ToString().Split(',');
                    secondDropDownItems.Add(itemInfo2[2].Split(':')[1].Replace("\"", "").Replace("}", "").Trim());
                }
            }

            if (firstDropDownItems.Count == 0 || secondDropDownItems.Count == 0)
            {
                MessageBox.Show("Failed to populate dropdown");
                FormHelper.mainScreen.Show();
                Hide();
                return false;
            }

            firstDropDown.DataSource = firstDropDownItems;
            secondDropDown.DataSource = secondDropDownItems;
            if (ids1 == null || ids2 == null) return true;
            ids1.Clear();
            ids2.Clear();

            if(collection == "HasDisability")
            {
                foreach(var item in itemSet1)
                {
                    ids1.Add(item.ToString().Split(',')[1].Split(':')[1].Replace("\"", "").Trim());
                }
                foreach (var item in itemSet2)
                {
                    ids2.Add(item.ToString().Split(',')[1].Split(':')[1].Replace("\"", "").Trim());
                }
            }
            else if(collection == "UsesAid")
            {
                foreach (var item in itemSet1)
                {
                    ids1.Add(item.ToString().Split(',')[1].Split(':')[1].Replace("\"", "").Trim());
                }
                foreach (var item in itemSet2)
                {
                    ids2.Add(item.ToString().Split(',')[1].Split(':')[1].Replace("\"", "").Trim());
                }
            }

            return true;
        }
    }
}
