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
    public partial class AddClientDisability : AddForm
    {
        List<String> disabilityIDS = new List<string>(),
            clientIDS = new List<string>();
        string queryDisabilities = "SELECT DISABILITYNAME, DISABILITYID FROM DISABILITY",
            queryClients = "SELECT FIRSTNAME || ' ' || LASTNAME, CLIENTID FROM CLIENT";

        public AddClientDisability()
        {
            InitializeComponent();
            SetupButtons();
        }

        public override void ShowForm()
        {
            base.ShowForm();
            
            if (StartScreen.radOracle.Checked)
            {
                if (!FillDropDown(queryDisabilities, "There are no disabilities in the database", ref disabilityIDS, ref disabilityDropDown)) return;
                FillDropDown(queryClients, "There are no clients in the database", ref clientIDS, ref clientDropDown);
            }
            else
            {
                FillMultiDropDown("HasDisability", ref clientIDS, ref disabilityIDS, ref clientDropDown, ref disabilityDropDown);
            }
        }

        public void btnSubmit_Click(object sender, EventArgs e)
        {
            string clientId = clientIDS[clientDropDown.SelectedIndex],
                disabilityId = disabilityIDS[disabilityDropDown.SelectedIndex],
                query = "INSERT INTO HASDISABILITY VALUES (HASDISABILITYAI.NEXTVAL, " + clientId + ", " + disabilityId + ")";

            if (StartScreen.radOracle.Checked)
            {
                if (SQLHelper.ExecuteModifyQuery(query) != -1)
                    ClearInput();
            }
            else
            {
                var hasDisabilityCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("HasDisability");
                var last = Builders<BsonDocument>.Filter.Empty;

                int idnum = 1;
                for (int i = 0; i < hasDisabilityCollection.Find(last).ToListAsync().Result.Count; i++)
                {
                    idnum++;
                }

                BsonDocument hasDisabilityDocument = new BsonDocument
                {
                    {"hasdisabilityid", new BsonInt32(idnum) },
                    {"clientid", new BsonInt32(int.Parse(clientId)) },
                    {"disabilityid", new BsonInt32(int.Parse(disabilityId)) }
                };
                hasDisabilityCollection.InsertOne(hasDisabilityDocument);
                MessageBox.Show("(MONGO) 1 Row Affected");
                ClearInput();
            }
        }

    }
}
