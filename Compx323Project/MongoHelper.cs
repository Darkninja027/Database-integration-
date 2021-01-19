using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;
using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System.Windows.Forms;

namespace Compx323Project
{
    static class MongoHelper
    {
        static MongoClient mongoClient = new MongoClient("mongodb://127.0.0.1:27017");
        public static IMongoDatabase mongoDB = mongoClient.GetDatabase("compx323-29");

        //static object projectDatabase = db.GetCollection<BsonDocument>("compx323-29");

        public static void ExecuteMongoDisplay(string collection)
        {
            IMongoCollection<BsonDocument> dbCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>(collection);
            FilterDefinition<BsonDocument> all = Builders<BsonDocument>.Filter.Empty;
            List<string> items = new List<string>();
            StartScreen.displayList.Items.Clear();
            List<int> paddings = new List<int>();
            List<string[]> stringArr = new List<string[]>();


            foreach (var item in dbCollection.Find(all).ToListAsync().Result)
            {
                string[] entry = item.ToString().Split(',');
                string[] properEntry = new string[entry.Length - 1];
                for (int i = 0; i < properEntry.Length; i++)
                {
                    properEntry[i] = entry[i + 1].Split(':')[1].Replace("\"", "").Replace("}", "").Trim();
                }
                stringArr.Add(properEntry);
            }

            if (collection == "Aids")
            {
                string[] header = { "Id", "Aid Type"};
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[1].PadRight(paddings[1]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add(info[1].PadRight(paddings[1]));
                }
            }
            else if (collection == "Caregiver")
            {
                string[] header = { "ID", "Name", "Name", "D.O.B", "Phone", "Company"  };
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[1].PadRight(paddings[1] + paddings[2] - 2) + " " + header[3].PadRight(paddings[3]) + " " + header[4].PadRight(paddings[4]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add((info[1] + " " + info[2]).PadRight(paddings[1] + paddings[2] - 2) + " " + info[3].PadRight(paddings[3]) + " " + info[4].PadRight(paddings[4]));
                }
            }
            else if (collection == "Client")
            {
                string[] header = { "Id", "Name", "Name", "", "", "", "" , "", "Phone", "D.O.B"};
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[1].PadRight(paddings[1] + paddings[2] - 2) + " " + header[8].PadRight(paddings[8]) + " " + header[9].PadRight(paddings[9]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add((info[1] + " " + info[2]).PadRight(paddings[1] + paddings[2] - 2) + " " + info[8].PadRight(paddings[8]) + " " + info[9].PadRight(paddings[9]));
                }
            }
            else if (collection == "Company")
            {
                string[] header = { "Company Name", "Owner", "", "", "", "", "Counrty", "Phone" };
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[0].PadRight(paddings[0]) + " " + header[1].PadRight(paddings[1]) + " " + header[7].PadRight(paddings[7]) + " " + header[6].PadRight(paddings[6]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add(info[0].PadRight(paddings[0]) + " " + info[1].PadRight(paddings[1]) + " " + info[7].PadRight(paddings[7]) + " " + info[6].PadRight(paddings[6]));
                }
            }
            else if (collection == "Disability")
            {
                string[] header = { "ID", "Name", "Area", "Description" };
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[1].PadRight(paddings[1]) + " " + header[2].PadRight(paddings[2]) + " " + header[3].PadRight(paddings[3]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add(info[1].PadRight(paddings[1]) + " " + info[2].PadRight(paddings[2]) + " " + info[3].PadRight(paddings[3]));
                }
            }
            else if (collection == "HasDisability")
            {
                string[] header = { "Name", "Disability" };
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                for (int i = 0; i < stringArr.Count; i++)
                {
                    stringArr[i] = new string[]{ GetClientName(stringArr[i][1]), GetDisabilityName(stringArr[i][2])};
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[0].PadRight(paddings[0]) + " " + header[1].PadRight(paddings[1]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add(info[0].PadRight(paddings[0]) + " " + info[1].PadRight(paddings[1]));
                }
            }
            else if (collection == "UsesAid")
            {
                string[] header = { "Name", "Aid" };
                foreach (string h in header)
                {
                    paddings.Add(h.Length + 2);
                }
                for (int i = 0; i < stringArr.Count; i++)
                {
                    stringArr[i] = new string[] { GetClientName(stringArr[i][1]), GetAidName(stringArr[i][2]) };
                }
                SetupPadding(stringArr, ref paddings);
                StartScreen.displayList.Items.Add(header[0].PadRight(paddings[0]) + " " + header[1].PadRight(paddings[1]));
                for (int i = 0; i < stringArr.Count; i++)
                {
                    string[] info = stringArr[i];
                    StartScreen.displayList.Items.Add(info[0].PadRight(paddings[0]) + " " + info[1].PadRight(paddings[1]));
                }
            }

            StartScreen.numberResults.Text = "Number of Results: " + stringArr.Count;
            items.Clear();
        }

        public static void SetupPadding(List<string[]> stringArr, ref List<int> paddings)
        {
            foreach (string[] entry in stringArr)
            {
                for (int i = 0; i < paddings.Count; i++)
                {
                    paddings[i] = (entry[i].Length + 2 > paddings[i]) ? entry[i].Length + 2 : paddings[i];
                }
            }
        }

        public static string GetClientName(string client)
        {
            var clientcoll = mongoDB.GetCollection<BsonDocument>("Client");
            var filter = Builders<BsonDocument>.Filter.Eq("clientid", int.Parse(client));
            var clientDetails = clientcoll.Find(filter).FirstOrDefault();
            string[] entry = clientDetails.ToString().Split(',');
            string fname = entry[2].Split(':')[1].Replace("\"", "").Replace("}", "").Trim();
            string lname = entry[3].Split(':')[1].Replace("\"", "").Replace("}", "").Trim();
            return fname + " " + lname;

        }

        public static string GetDisabilityName(string id)
        {
            var disacoll = mongoDB.GetCollection<BsonDocument>("Disability");
            var filter = Builders<BsonDocument>.Filter.Eq("disabilityid", int.Parse(id));
            var disaDetails = disacoll.Find(filter).FirstOrDefault();
            string[] entry = disaDetails.ToString().Split(',');
            string disaName = entry[2].Split(':')[1].Replace("\"", "").Replace("}", "").Trim();
            return disaName;
        }

        public static string GetAidName(string id)
        {
            var aidcoll = mongoDB.GetCollection<BsonDocument>("Aids");
            var filter = Builders<BsonDocument>.Filter.Eq("aidid", int.Parse(id));
            var aidDetails = aidcoll.Find(filter).FirstOrDefault();
            string[] entry = aidDetails.ToString().Split(',');
            string aidName = entry[2].Split(':')[1].Replace("\"", "").Replace("}", "").Trim();
            return aidName;
        }
    }
}
