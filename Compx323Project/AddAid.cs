using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compx323Project
{
    public partial class AddAid : AddForm
    {
        public AddAid()
        {
            InitializeComponent();
            SetupButtons();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //added if statement to see what kind of add statement it is #BRENT#
            string aidName = FormHelper.FormatWords(txtAid.Text.Trim());

            if (FormHelper.StringError(aidName, "Please Enter a valid Aid.")) return;

            if (StartScreen.radOracle.Checked) {

                if (SQLHelper.ExecuteModifyQuery("INSERT INTO AIDS VALUES(AIDAI.NEXTVAL,'" + aidName + "')") != -1)
                    ClearInput();
            }
            else
            {
                var aidCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Aids");
                var last = Builders<BsonDocument>.Filter.Empty;
                
                int idnum = 1;
                for(int i = 0; i < aidCollection.Find(last).ToListAsync().Result.Count; i++)
                {
                    idnum++;
                }
                BsonDocument aidDocument = new BsonDocument()
                {
                    {"aidid", new BsonInt32(idnum) },
                    {"aidtype", aidName }
                };
                aidCollection.InsertOne(aidDocument);
                MessageBox.Show("(MONGO) 1 Row Affected");
                ClearInput();
                //Console.WriteLine(idnum);


               
            }
            
        }
    }
}