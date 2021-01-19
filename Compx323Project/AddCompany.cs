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
    public partial class AddCompany : ModifiableForm
    {
        public AddCompany()
        {
            InitializeComponent();
            SetupButtons();
        }

        public override void UpdateForm()
        {
            base.UpdateForm();
            txtName.Enabled = false;
            string companyName = SQLHelper.identifiers[StartScreen.displayList.SelectedIndex - 1];
            DataTable table = SQLHelper.GetDataTable("SELECT * FROM COMPANY WHERE COMPANYNAME = '" + companyName + "'");
            txtName.Text = companyName;
            txtOwner.Text = table.Rows[0][1].ToString();
            txtStreetNumber.Text = table.Rows[0][2].ToString();
            txtStreetName.Text = table.Rows[0][3].ToString();
            txtCity.Text = table.Rows[0][4].ToString();
            txtPostcode.Text = table.Rows[0][5].ToString();
            txtCountry.Text = table.Rows[0][6].ToString();
            txtPhone.Text = table.Rows[0][7].ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = FormHelper.FormatWords(txtName.Text.Trim()),
                owner = FormHelper.FormatWords(txtOwner.Text.Trim()),
                streetNumber = FormHelper.FormatNumber(txtStreetNumber.Text.Trim()),
                streetName = FormHelper.FormatAlphanumericWords(txtStreetName.Text.Trim()),
                postcode = FormHelper.FormatNumber(txtPostcode.Text.Trim()),
                country = FormHelper.FormatWord(txtCountry.Text.Trim()),
                city = FormHelper.FormatWords(txtCity.Text.Trim()),
                phone = FormHelper.FormatNumber(txtPhone.Text.Trim());

            if (FormHelper.StringError(name, "Please enter valid company name.")) return;
            if (FormHelper.StringError(owner, "Please enter valid name.")) return;
            if (FormHelper.StringError(streetNumber, "Please enter valid street number.")) return;
            if (FormHelper.StringError(streetName, "Please enter valid street name.")) return;
            if (FormHelper.StringError(postcode, "Please enter valid postcode.")) return;
            if (FormHelper.StringError(country, "Please enter valid country.")) return;
            if (FormHelper.StringError(city, "Please enter valid city.")) return;
            if (FormHelper.StringError(phone, "Please enter valid phone number.")) return;

            string query;

            if (StartScreen.radOracle.Checked)
            {
                if (updating)
                    query = "UPDATE COMPANY SET COMPANYOWNER = '" + owner + "', STREETNUMBER = " + streetNumber + ", STREETNAME = '" + streetName + "', CITY = '" + city + "', POSTCODE = " + postcode + ", COUNTRY = '" + country + "', PHONE = " + phone + " WHERE COMPANYNAME = '" + txtName.Text + "'";
                else
                    query = "INSERT INTO COMPANY VALUES('" + name + "', '" + owner + "', " + streetNumber + ", '" + streetName + "', '" + city + "', " + postcode + ", '" + country + "', " + phone + ")";

                if (SQLHelper.ExecuteModifyQuery(query) != -1)
                {
                    ClearInput();
                    if (updating) FormExit(null, null);
                }
            }
            else
            {

                var companyCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Company");
                BsonDocument newCompany = new BsonDocument
                {
                    {"company", name },
                    {"companyowner", owner },
                    {"streetnumber", new BsonInt32(int.Parse(streetNumber))},
                    {"streetname", streetName },
                    {"city", city},
                    {"postcode", new BsonInt32(int.Parse(postcode)) },
                    {"country", country },
                    {"phone", new BsonInt32(int.Parse(phone)) }
                };
                companyCollection.InsertOne(newCompany);
                MessageBox.Show("(MONGO) 1 Row Affected");
                ClearInput();

            }

            
        }
    }
}
