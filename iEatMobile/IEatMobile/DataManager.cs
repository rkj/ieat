using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Xml;
using System.Windows.Forms;

namespace IEatMobile
{
    class DataManager
    {
        private System.Threading.Mutex mutex = new System.Threading.Mutex();
        //TODO zamienic na singletony
        private XmlSerializer productSerializer = new XmlSerializer(typeof(Product));
        private XmlSerializer recipeSerializer = new XmlSerializer(typeof(Recipe));

        private XmlSerializer productsListSerializer = new XmlSerializer(typeof(ProductsList));
        private XmlSerializer recipesListSerializer = new XmlSerializer(typeof(RecipesList));
        private XmlSerializer fridgeSerializer = new XmlSerializer(typeof(Fridge));

        private string ProductsFile = "/Program Files/iEatMobile/ProductsList.xml";
        private string RecipesFile = "/Program Files/iEatMobile/RecipesList.xml";
        private string RecipeFile = "/Program Files/iEatMobile/Recipe";
        private string FridgeFile = "/Program Files/iEatMobile/Fridge.xml";
        private string FridgesFile = "/Program Files/iEatMobile/Fridges.xml";
        
        private ProductsList productsList;
        private RecipesList recipesList;
        private FridgeList fridgeList;
        private Fridge fridge;
        private string cookie;

        public ProductsList ProductsList
        {
            get { return productsList; }
            set { productsList = value; }
        }
        
        public RecipesList RecipesList
        {
            get { return recipesList; }
            set { recipesList = value; }
        }
        
        public List<FridgeProduct> Fridge
        {
            get { return fridge.Products; }
            set { fridge.Products = value; }
        }

        public DataManager()
        {
            /*
            if (File.Exists(RecipesFile))
            {
                StreamReader myReader = new StreamReader(RecipesFile);
                recipesList = (RecipesList)recipesListSerializer.Deserialize(myReader);
                myReader.Close();
            }
            else
                recipesList = new RecipesList();

            if (File.Exists(FridgeFile))
            {
                StreamReader myReader = new StreamReader(FridgeFile);
                fridge = (Fridge)fridgeSerializer.Deserialize(myReader);
                myReader.Close();
            }
            else
                fridge = new Fridge();

            if (File.Exists(ProductsFile))
            {
                StreamReader myReader = new StreamReader(ProductsFile);
                productsList = (ProductsList)productsListSerializer.Deserialize(myReader);
                myReader.Close();
            }
            else
                productsList = new ProductsList();
            */

            recipesList = new RecipesList();
            fridge = new Fridge();
            productsList = new ProductsList();
        }
        public void loadProducts()
        {
            XmlTextReader xmlr = new XmlTextReader(ProductsFile);
            productsList.Deserialize(xmlr);
            xmlr.Close();
        }
        public void loadFridges()
        {
            XmlTextReader xmlr = new XmlTextReader(FridgesFile);
            XmlSerializer xs = new XmlSerializer(typeof(FridgeList));
            fridgeList = (FridgeList)xs.Deserialize(xmlr);
        }
        public void loadFridge()
        {
            XmlTextReader xmlr = new XmlTextReader(FridgeFile);
            XmlSerializer xs = new XmlSerializer(typeof(Fridge));
            fridge = (Fridge)xs.Deserialize(xmlr);
        }
        public void loadRecipesList()
        {
            XmlTextReader xmlr = new XmlTextReader(RecipesFile);
            recipesList.Deserialize(xmlr);
            xmlr.Close();
        }
        public void loadRecipes()
        {
            List<Recipe> rList = new List<Recipe>();
            foreach (RecipeForList r in recipesList.ListedRecipies)
            {
                Recipe re = new Recipe();
                XmlTextReader xmlr = new XmlTextReader(RecipeFile+r.id+".xml");
                re.Deserialize(xmlr);
                xmlr.Close();

             //   for (int i = 0; i < re.Steps.Count; i++)
               //     for (int j = 0; j < re.Steps[i].Ingredients.Count; j++)
                 //       re.Steps[i].Ingredients[j].Product = productsList.getById(re.Steps[i].Ingredients[j].ProductId);
               
                rList.Add(re);
            }
            recipesList.Recipes = rList;
        }
        public void save()
        {
            StreamWriter myWriter = new StreamWriter(RecipesFile, false);
            recipesListSerializer.Serialize(myWriter, recipesList);
            myWriter.Close();

            myWriter = new StreamWriter(FridgeFile, false);
            fridgeSerializer.Serialize(myWriter, recipesList);
            myWriter.Close();
        }


        public void synchronize()
        {
            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress+"/ieat/recipe.xml");
            HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

            StreamReader sr = new StreamReader(HttpWResp.GetResponseStream());
            Recipe newRecipe = (Recipe)recipeSerializer.Deserialize(sr);

            HttpWResp.Close();

            recipesList.Recipes.Add(newRecipe);
        }

        public void saveProductsList()
        {
            StreamWriter myWriter = new StreamWriter(ProductsFile, false);
            productsListSerializer.Serialize(myWriter, productsList);
            myWriter.Close();
        }
        public void saveFridgesList()
        {
            StreamWriter myWriter = new StreamWriter(FridgesFile, false);
            XmlSerializer xs = new XmlSerializer(typeof(FridgeList));
            xs.Serialize(myWriter, fridgeList);
            myWriter.Close();
        }
        public void saveFridge()
        {
            StreamWriter myWriter = new StreamWriter(FridgeFile, false);
            XmlSerializer xs = new XmlSerializer(typeof(Fridge));
            xs.Serialize(myWriter,fridge);
            myWriter.Close();
        }
        public void saveRecipesList()
        {
            StreamWriter myWriter = new StreamWriter(RecipesFile, false);
            recipesListSerializer.Serialize(myWriter, recipesList);
            myWriter.Close();
        }
        public void saveRecipes()
        {
            foreach (Recipe r in recipesList.Recipes)
            {
                XmlTextWriter myWriter = new XmlTextWriter(RecipeFile + r.Id + ".xml",Encoding.UTF8);
                r.Serialize(myWriter);
                myWriter.Close();
            }
        }
        public bool login()
        {
            try
            {
                /*HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress+"/users/login";);
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                cookie = resp.Headers["Set-Cookie"];
                string location = config.Current.serverAddress + "/users/login";
                //MessageBox.Show(resp.Headers["Set-Cookie"]);
                resp.Close();
                */


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress + "/users/login");
                req.KeepAlive = true;
                req.PreAuthenticate = true;
               // req.Headers["Cookie"] = cookie;
                req.AllowAutoRedirect = true;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string page = sr.ReadToEnd();
                string authPhrase = "name=\"authenticity_token\" type=\"hidden\" value=\"";
                int start = page.IndexOf(authPhrase) + authPhrase.Length;
                int end = page.IndexOf('"', start);
                string authCode = page.Substring(start, end - start);
                cookie = resp.Headers["Set-Cookie"];
                resp.Close();



                req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress + "/users/do_login");
                req.Method = "POST";

                string loginMessage = "authenticity_token=" + authCode + "&login=" + "DummyBoy" + "&password=" + "bursztynek" + "&remember_me=1&commit=Log+in";
                // HttpWReq.ContentLength = loginMessage.Length;
                req.ContentLength = loginMessage.Length;
                req.PreAuthenticate = true;
                req.AllowAutoRedirect = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("Cookie", cookie);
                req.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                req.AllowWriteStreamBuffering = true;
                req.Referer = config.Current.serverAddress + "/users/login";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.8.1.12) Gecko/20080201 Firefox/2.0.0.12";
                Version v = new Version("1.1");
                req.ProtocolVersion = v;
                // HttpWReq.
                byte[] bytes = Encoding.ASCII.GetBytes(loginMessage);
                Stream s = req.GetRequestStream();
                s.Write(bytes, 0, bytes.Length);
               

                s.Close();


                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                
                cookie = HttpWResp.Headers["Set-Cookie"];
                sr = new StreamReader(HttpWResp.GetResponseStream());
                page = sr.ReadToEnd();
                for (int i = 0; i < HttpWResp.Headers.Count; i++)
                    MessageBox.Show("Key: " + HttpWResp.Headers.Keys[i] + "\nValue: " + HttpWResp.Headers[i]);
                HttpWResp.Close();

                cookie += ";auth_token=" + authCode;
                req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress);
                req.PreAuthenticate = true;
                req.AllowAutoRedirect = true;
                req.AllowWriteStreamBuffering = true;
                HttpWResp = (HttpWebResponse)req.GetResponse();
                return true;

            }
            catch (WebException ee)
            {
             
                MessageBox.Show(ee.Message);
                return false;
            }
        }
        Recipe getRecipe(int id)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress + "/synchro/recipe/"+id);
               
                req.AllowWriteStreamBuffering = true;
                req.KeepAlive = true;
                req.Method = "Get";
                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                XmlTextReader xmltr = new XmlTextReader(HttpWResp.GetResponseStream());
                Recipe recipe = new Recipe();
                recipe.Deserialize(xmltr);
                HttpWResp.Close();

                for (int i = 0; i < recipe.Steps.Count; i++)
                    for (int j = 0; j < recipe.Steps[i].Ingredients.Count; j++)
                        recipe.Steps[i].Ingredients[j].Product = productsList.getById(recipe.Steps[i].Ingredients[j].ProductId);
               
                return recipe;
            }
            catch (WebException ee)
            {
                /*
                WebResponse resp = ee.Response;
                
                StreamReader sr=new StreamReader(resp.GetResponseStream());
                string allresp=sr.ReadToEnd();
                string respmes = ee.Message;
           
                 */
                MessageBox.Show(ee.Message);
                return null;
            }
        }
      
        public void getRecipesList()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress + "/synchro/recipes_list");
                req.Headers["Cookie"] = cookie;
                req.AllowWriteStreamBuffering = true;
                req.KeepAlive = true;
                req.Method = "Get";
                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                XmlTextReader xmltr = new XmlTextReader(HttpWResp.GetResponseStream());
                RecipesList list = new RecipesList();
                list.Deserialize(xmltr);
                HttpWResp.Close();

                recipesList = list;
                List<Recipe> rList = new List<Recipe>();
                foreach (RecipeForList r in recipesList.ListedRecipies)
                {
                    Recipe re = getRecipe(r.id);
                    rList.Add(re);
                }
                recipesList.Recipes = rList;
            }
            catch (WebException ee)
            {
                /*
                WebResponse resp = ee.Response;
                
                StreamReader sr=new StreamReader(resp.GetResponseStream());
                string allresp=sr.ReadToEnd();
                string respmes = ee.Message;
           
                 */
                MessageBox.Show(ee.Message);
            }
        }
        public void getProductsList()
        {
            try
            {
              //  if (cookie == null)
                //    if (!login())
                  //      return;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress+"/synchro/products_list");
                req.Headers["Cookie"] = cookie;
                req.AllowWriteStreamBuffering = true;
                req.KeepAlive = true;
                req.Method = "Get";
                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                XmlTextReader xmltr = new XmlTextReader(HttpWResp.GetResponseStream());
                ProductsList list = new ProductsList();
                list.Deserialize(xmltr);
                HttpWResp.Close();
                
                //mutex.WaitOne(5000, true);
                productsList = list;
                //mutex.ReleaseMutex();
            }
            catch (WebException ee)
            {
                /*
                WebResponse resp = ee.Response;
                
                StreamReader sr=new StreamReader(resp.GetResponseStream());
                string allresp=sr.ReadToEnd();
                string respmes = ee.Message;
           
                 */
                MessageBox.Show(ee.Message);
            }
        }

        public void getFridgeList()
        {
            try
            {
                //  if (cookie == null)
                //    if (!login())
                //      return;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress + "/containers.xml?user="+config.Current.user);
                req.Headers["Cookie"] = cookie;
                req.AllowWriteStreamBuffering = true;
                req.KeepAlive = true;
                req.Method = "Get";
                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                XmlTextReader xmltr = new XmlTextReader(HttpWResp.GetResponseStream());
                FridgeList list = new FridgeList();
                list.Deserialize(xmltr);
                HttpWResp.Close();

                //mutex.WaitOne(5000, true);
                fridgeList = list;
                //mutex.ReleaseMutex();
            }
            catch (WebException ee)
            {
                /*
                WebResponse resp = ee.Response;
                
                StreamReader sr=new StreamReader(resp.GetResponseStream());
                string allresp=sr.ReadToEnd();
                string respmes = ee.Message;
           
                 */
                MessageBox.Show(ee.Message);
            }
        }
        public void getFridge()
        {
            try
            {
                //  if (cookie == null)
                //    if (!login())B
                //      return;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(config.Current.serverAddress + "/containers/"+fridgeList.containers[0].id+".xml");
                req.Headers["Cookie"] = cookie;
                req.AllowWriteStreamBuffering = true;
                req.KeepAlive = true;
                req.Method = "Get";
                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                XmlTextReader xmltr = new XmlTextReader(HttpWResp.GetResponseStream());
                    XmlSerializer xs  = new XmlSerializer(typeof(IEatMobile.Fridge));
                    fridge = (IEatMobile.Fridge)xs.Deserialize(xmltr);
                    List<FridgeProduct> fps = new List<FridgeProduct>();
                    for (int i = 0; i < fridge.Products.Count; i++)
                    {
                        FridgeProduct fp = fridge.Products[i];
                        fp.Product = productsList.getById(fp.ProductId);
                        fps.Add(fp);
                        
                    }
                    fridge.Products = fps;
              
            }
            catch (WebException ee)
            {
                /*
                WebResponse resp = ee.Response;
                
                StreamReader sr=new StreamReader(resp.GetResponseStream());
                string allresp=sr.ReadToEnd();
                string respmes = ee.Message;
           
                 */
                MessageBox.Show(ee.Message);
            }
        }
        public void fillWithTestData()
        {
            recipesList.fillWithData();
            fridge.fillWithTest();
            productsList.fillWithTest();
        }
    }
}
