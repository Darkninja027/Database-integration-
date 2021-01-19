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
    public partial class addLoan : AddForm
    {
        List<String> clientIDS = new List<string>(),
            aidTypes = new List<string>();
        string queryClient = "SELECT FIRSTNAME || ' ' || LASTNAME, CLIENTID FROM CLIENT",
            queryAids = "SELECT DISTINCT AIDTYPE || ' (' || COUNT(AIDTYPE) || ')', AIDTYPE FROM (SELECT * FROM AIDS MINUS (SELECT A.AIDID, A.AIDTYPE FROM AIDS A, USESAID U WHERE U.AIDID = A.AIDID)) GROUP BY AIDTYPE ORDER BY AIDTYPE || ' (' || COUNT(AIDTYPE) || ')'";

        public addLoan()
        {
            InitializeComponent();
            SetupButtons();
        }

        public override void ShowForm()
        {
            base.ShowForm();
            if (StartScreen.radOracle.Checked)
            {
                if (!FillDropDown(queryClient, "There are no clients in the database", ref clientIDS, ref clientCombo)) return;
                FillDropDown(queryAids, "There are no aids available in the database", ref aidTypes, ref aidCombo);
            }
            else
            {
                FillMultiDropDown("UsesAid", ref clientIDS, ref aidTypes, ref clientCombo, ref aidCombo);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string clientID = clientIDS[clientCombo.SelectedIndex],
                aidType = aidTypes[aidCombo.SelectedIndex],
                getQuery = "SELECT MIN(AIDID) FROM (SELECT * FROM AIDS MINUS(SELECT A.AIDID, A.AIDTYPE FROM AIDS A, USESAID U WHERE U.AIDID = A.AIDID)) WHERE AIDTYPE = '" + aidType + "'",
                aidID = SQLHelper.GetDataTable(getQuery).Rows[0][0].ToString(),
                addQuery = "INSERT INTO USESAID VALUES(usesaidai.nextval, " + clientID + ", " + aidID + ")";
            if (StartScreen.radOracle.Checked)
            {
                if (SQLHelper.ExecuteModifyQuery(addQuery) != -1)
                    ClearInput();
            }
            else
            {
                var loanCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("UsesAid");
                var last = Builders<BsonDocument>.Filter.Empty;

                int idnum = 1;
                for (int i = 0; i < loanCollection.Find(last).ToListAsync().Result.Count; i++)
                {
                    idnum++;
                }
                BsonDocument loanDocument = new BsonDocument
                {
                    {"usesid", new BsonInt32(idnum) },
                    {"clientid", new BsonInt32(int.Parse(clientID)) },
                    {"aidid", new BsonInt32(int.Parse(aidType)) }
                };
                loanCollection.InsertOne(loanDocument);
                MessageBox.Show("(Mongo) 1 Row Affected");
                ClearInput();
            }
        }
    }
}