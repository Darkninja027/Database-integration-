using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compx323Project
{
    public partial class AddDisability : AddForm
    {
        public AddDisability()
        {
            InitializeComponent();
            SetupButtons();
        }

        public void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = FormHelper.FormatWord(txtName.Text.Trim()),
                area = FormHelper.FormatWord(txtArea.Text.Trim()),
                description = txtDescription.Text.Trim();

            if (FormHelper.StringError(name, "Please enter valid name.")) return;
            if (FormHelper.StringError(area, "Please enter valid area.")) return;
            if (FormHelper.StringError(description, "Please enter a description.")) return;

            if (StartScreen.radOracle.Checked)
            {
                if (SQLHelper.ExecuteModifyQuery("insert into disability values(disabilityai.nextval, '" + name + "', '" + area + "', '" + description + "')") != -1)
                    ClearInput();
            }
            else
            {
                var disabilityCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Disability");
                var last = Builders<BsonDocument>.Filter.Empty;

                int idnum = 1;
                for (int i = 0; i < disabilityCollection.Find(last).ToListAsync().Result.Count; i++)
                {
                    idnum++;
                }

                BsonDocument disabilityDocument = new BsonDocument
                {
                    {"disabilityid", new BsonInt32(idnum) },
                    {"disabilityname", name },
                    {"area", area },
                    {"description", description }
                };
                disabilityCollection.InsertOne(disabilityDocument);
                MessageBox.Show("(MONGO) 1 Row Affected");
                ClearInput();
            }
        }
    }
}
