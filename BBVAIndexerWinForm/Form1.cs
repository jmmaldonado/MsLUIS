using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Configuration;


using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;

namespace BBVAIndexerWinForm {




    public partial class Form1 : Form {


        private List<String> _listaFicheros;
        XmlDocument XMLDoc;

        List<Uri> indexedFilesList;
        string _LUISQuery;
        int _LUISMaxCharacters;
        bool _verbose;


        


        public Form1() {
            InitializeComponent();
            _listaFicheros = new List<string>();
            _listaFicheros.Clear();

            _LUISQuery = "https://api.projectoxford.ai/luis/v1/application?id=" + Properties.Settings.Default.LUISAppID + "&subscription-key=" + Properties.Settings.Default.LUISKey + "&q=";

            XMLDoc = new XmlDocument();
            _LUISMaxCharacters = Properties.Settings.Default.LUISMaxCharacters;
            _verbose = Properties.Settings.Default.VerboseLogging;
            //XDoc = new XDocument();
        }




        private void botSelectFolders_Click(object sender, EventArgs e) {

            //Para evitar tener que navegar demasiado - los ficheros estan en la carpeta SMIs
            fbd.SelectedPath = "C:\\Users\\jomaldon\\OneDrive - Microsoft\\CLIENTES\\BBVA\\Indexer (Pedro Suja)\\PoC";

            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK) {

                textSelectedFolder.Text = fbd.SelectedPath;
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                foreach (string fich in files)
                    _listaFicheros.Add(fich);

                rellenaListaFicheros();
            }
        }










        private void botGo_Click(object sender, EventArgs e) {

            if (listFiles.SelectedItem != null) {
                XMLDoc.Load(_listaFicheros[listFiles.SelectedIndex]);
                //XDoc = XDocument.Load(_listaFicheros[listFiles.SelectedIndex]);

                logOperation("Leyendo fichero TTML", XMLDoc.InnerXml.ToString());

                logOperation("Obteniendo Full Text", getTTMLFullText(XMLDoc));
            }

            

        }





        #region PRUEBAS
        ///////////////////////////////////////////////////////////
        // PRUEBAS
        ///////////////////////////////////////////////////////////


        private void pruebaLlamadaJSON() {

            string query = "https://api.projectoxford.ai/luis/v1/application?id=62690d1a-2574-4739-8d66-e2fdfc4adde1&subscription-key=0fc0fa6f97604d12ab9f5fa276f01fe4&q=quiero%20enviar%20seiscientos%20euros%20a%20Luis";

            WebClient webClient = new WebClient();
            object test = JsonConvert.DeserializeObject(webClient.DownloadString(query));
            dynamic stuff = test;
            string text = test.ToString();
            /*
            string name = stuff.Name;
            string address = stuff.Address.City;*/

            logOperation("Resultado del JSON", text);


            EnviarMensajeAServiceBus(test);
        }



        private void botTestReadFromBlob_Click(object sender, EventArgs e) {


            Thread readfilesThread = new Thread(new ThreadStart(processIndexedFilesFromStorageAccount));
            readfilesThread.Start();

        }



        private object getLUISResults(string text) {

            //_LUISQuery +=  Uri.EscapeDataString(text);
            string completeQuery = _LUISQuery + text;

            if (_verbose) logOperation("[VERBOSE] getLUISResults: URL para llamar a LUIS", completeQuery);

            WebClient webClient = new WebClient();
            object LUISResult = JsonConvert.DeserializeObject(webClient.DownloadString(completeQuery));
            dynamic stuff = LUISResult;
            string LUISResultText = LUISResult.ToString();
 
            if (_verbose) logOperation("[VERBOSE] getLUISResults: Resultado del JSON obtenido de LUIS", LUISResultText);

            return LUISResult;
            //EnviarMensajeAServiceBus(test);
        }





        private void botTestJSON2_Click(object sender, EventArgs e) {


        }







        ///////////////////////////////////////////////////////////
        #endregion












































        private void botAuto_Click(object sender, EventArgs e) {
            pruebaLlamadaJSON();
        }



        #region DATA CONTRACT para el JSON de respuesta de LUIS

        // Type created for JSON at <<root>>
        [System.Runtime.Serialization.DataContractAttribute()]
        public partial class LUISResponse {

            [System.Runtime.Serialization.DataMemberAttribute()]
            public string query;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public Intents[] intents;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public Entities[] entities;
        }

        // Type created for JSON at <<root>> --> intents
        [System.Runtime.Serialization.DataContractAttribute(Name = "intents")]
        public partial class Intents {

            [System.Runtime.Serialization.DataMemberAttribute()]
            public string intent;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public double score;
        }

        // Type created for JSON at <<root>> --> entities
        [System.Runtime.Serialization.DataContractAttribute(Name = "entities")]
        public partial class Entities {

            [System.Runtime.Serialization.DataMemberAttribute()]
            public string entity;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public string type;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public int startIndex;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public int endIndex;

            [System.Runtime.Serialization.DataMemberAttribute()]
            public double score;
        }


        #endregion


        #region CODIGO ACABADO




        private LUISResponse getLUISResultsAsObject(string text) {

            LUISResponse result;

            string completeQuery = _LUISQuery + text;

            if (_verbose) logOperation("[VERBOSE] getLUISResultsAsObject: URL para llamar a LUIS", completeQuery);

            WebClient webClient = new WebClient();
            string LUISStringResponse = webClient.DownloadString(completeQuery);

            //object LUISResult = JsonConvert.DeserializeObject(LUISStringResponse);


            DataContractJsonSerializer myjsonhelper = new DataContractJsonSerializer(typeof(LUISResponse));
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(LUISStringResponse))) {
                result = (LUISResponse)myjsonhelper.ReadObject(stream);
            }

            if (_verbose) logOperation("[VERBOSE] getLUISResultsAsObject: Resultado del JSON obtenido de LUIS", LUISStringResponse);

            return result;

        }




        private Intents[] removeDuplicateIntentsFromArray(Intents[] intents) {

            Hashtable table = new Hashtable();
            foreach (Intents intent in intents) {
                if (table.ContainsKey(intent.intent)) {
                    //De todos los intents iguales que encontremos nos quedamos con el de mayor score
                    if (((Intents)table[intent.intent]).score < intent.score) {
                        table[intent.intent] = intent;
                    }
                } else {
                    table.Add(intent.intent, intent);
                }
            }

            List<Intents> result = new List<Intents>();
            foreach (object resultIntent in table.Values)
                result.Add((Intents)resultIntent);

            return result.ToArray();
        }


        private Entities[] removeDuplicateEntitiesFromArray(Entities[] entities) {

            Hashtable table = new Hashtable();
            foreach (Entities entity in entities) {
                if (table.ContainsKey(entity.entity)) {
                    //De todos los entities iguales que encontremos nos quedamos con el de mayor score
                    if (((Entities)table[entity.entity]).score < entity.score) {
                        table[entity.entity] = entity;
                    }
                } else {
                    table.Add(entity.entity, entity);
                }
            }

            List<Entities> result = new List<Entities>();
            foreach (object resultEntity in table.Values)
                result.Add((Entities)resultEntity);

            return result.ToArray();
        }


        private LUISResponse composeFragmentedLUISResponses(List<LUISResponse> responseList) {
            LUISResponse result = new LUISResponse();
            result.query = "";
            result.intents = new List<Intents>().ToArray();
            result.entities = new List<Entities>().ToArray();

            foreach (LUISResponse response in responseList) {
                result.query += response.query + " ";
                result.intents = result.intents.ToList().Concat(response.intents.ToList()).ToArray();
                result.entities = result.entities.ToList().Concat(response.entities.ToList()).ToArray();
            }



            //TODO: Filtar los elementos que sean iguales de entities e intents.


            return result;
        }





        /// <summary>
        /// Invoca a LUIS para obtener el JSON Resultante con los intents y entities de la consulta
        /// Si el parámetro text tiene más de 500 caracteres se encarga de hacer el fragmentado y componer los resultados
        /// </summary>
        /// <param name="text">Texto plano para enviarle a LUIS</param>
        /// <returns>Objeto con el datacontract del JSON resultante de la llamada de Luis</returns>
        private LUISResponse makeLUISCallFromText(string text) {
            try {

                List<LUISResponse> LUISResultObjectList = new List<LUISResponse>();

                //Comprobar si el texto tiene más de [Settings Numero Caracteres] caracteres, en ese caso fragmentarlo en los chunks correspondientes
                //Si el texto es menor, sólo hace falta una llamada a LUIS
                //TODO: Ojo que al hacer el EscapeDataString en getLUISResults se meten muchos caracteres nuevos por el escapado de acentos, etc...
                if (text.Length <= _LUISMaxCharacters) {
                    //getLUISResults(text);
                    LUISResultObjectList.Add(getLUISResultsAsObject(text));

                } else {

                    //Necesitamos fragmentar el texto y recomponer los resultados
                    int fragments = (int)Math.Ceiling((decimal)text.Length / _LUISMaxCharacters);
                    int startIndex, numChars;
                    for (int fragment = 0; fragment < fragments; fragment++) {
                        startIndex = fragment * _LUISMaxCharacters;

                        //Corregimos el numero de caracteres por si en el ultimo fragmento nos pasamos de la longitud del texto
                        numChars = startIndex + _LUISMaxCharacters > text.Length ? text.Length - startIndex : _LUISMaxCharacters;

                        //getLUISResults(text.Substring(startIndex, numChars));
                        LUISResultObjectList.Add(getLUISResultsAsObject(text.Substring(startIndex, numChars)));
                    }

                }

                //Componer el resultado de varias llamadas a LUIS a una consolidada y eliminar duplicados en intents y entities
                LUISResponse response = composeFragmentedLUISResponses(LUISResultObjectList);
                Intents[] uniqueIntents = removeDuplicateIntentsFromArray(response.intents);
                Entities[] uniqueEntities = removeDuplicateEntitiesFromArray(response.entities);
                response.intents = uniqueIntents;
                response.entities = uniqueEntities;

                return response;

            } catch (Exception ex) { logOperation("[ERROR] Excepcion en makeLUISCallFromText", ex.Message); return null; }
        }










        private void processIndexedFilesFromStorageAccount() {

            try {

                indexedFilesList = new List<Uri>();
                indexedFilesList.Clear();

                string storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=" + Properties.Settings.Default.IndexedFilesStorageAccountName + ";AccountKey=" + Properties.Settings.Default.IndexedFilesStorageAccountKey;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                IEnumerable<CloudBlobContainer> containerList = blobClient.ListContainers();

                //Listado de todos los contenedores
                
                foreach (CloudBlobContainer container in containerList) {

                    //Listado de todos los blobs dentro del contenedor actual
                    IEnumerable<IListBlobItem> blobList = container.ListBlobs();
                    foreach (IListBlobItem blob in blobList) {

                        string filename = blob.Uri.Segments.Last();
                        string[] filenameArray = filename.Split('.');

                        //Nos quedamos con el fichero a procesar si coincide con la extensión configurada en los settings de la aplicacion ( json )
                        if (filenameArray[filenameArray.Length - 1].Equals(Properties.Settings.Default.IndexedFilesExtension, StringComparison.OrdinalIgnoreCase)) {
                            indexedFilesList.Add(blob.Uri);
                            logOperation("Indexed file found", blob.Uri.Segments.Last());


                            //Leer los contenidos del BLOB actual
                            string contenidoBlob = readBlobFileAsText(container, blob.Uri);
                            if (_verbose) logOperation("[VERBOSE] Raw contents of the file", contenidoBlob);


                            //Extraer el texto del JSON a un String plano con la transcripcion 
                            string textoBlob = "";
                            if (contenidoBlob != "") {
                                JArray ja = JArray.Parse(contenidoBlob);
                                foreach (JToken jt in ja) {
                                    textoBlob += jt.First.First.ToString() + " ";
                                }
                                if (_verbose) logOperation("[VERBOSE] Parsed contents of the file", textoBlob);
                            }


                            //Hacer las llamadas a LUIS, el propio método se encargará de hacer las particiones y agrupar los resultados fragmentados
                            //TODO: Convertir el objeto LUISResponse en JSON para enviar al servicebus
                            if (textoBlob != "") {
                                LUISResponse luisResponse;
                                luisResponse = makeLUISCallFromText(textoBlob);
                                if (luisResponse != null) {
                                    EnviarMensajeAServiceBus(luisResponse);
                                }
                            }


                        }
                    }
                }

            } catch (Exception ex) {
                logOperation("[ERROR] Excepcion en getIndexedFilesFromStorageAccount", ex.Message.ToString());
            }
     

        }





        private string readBlobFileAsText(CloudBlobContainer container, Uri blob) {
            try {
                string result = "";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob.Segments.Last());
                using (var memoryStream = new MemoryStream()) {
                    blockBlob.DownloadToStream(memoryStream);
                    result = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                return result;
            } catch (Exception ex) { logOperation("[ERROR] Excepcion en readBlobFileAsText", ex.Message); return ""; }
        }







        private void EnviarMensajeAServiceBus(object mensaje) {


            //Generar SAS Token
            /* SOLO NECESARIO PARA ENVIAR VIA REST POST - CON EL CLIENTE ES MAS SENCILLO
            var sbNamespace = "bbvaeventhub-ns";
            var sbPath = "bbvaeventhub";
            var sbPolicy = "winformApp";
            var sbKey = "OuUNs2AMtnOdZwgiXHrVk3k8KPAjWIZ6EkGr3Nnc1io=";
            var expiry = new TimeSpan(100, 1, 1, 1);

            var serviceUri = ServiceBusEnvironment.CreateServiceUri("https", sbNamespace, sbPath).ToString().Trim('/');
            var generatedSas = SharedAccessSignatureTokenProvider.GetSharedAccessSignature(sbPolicy, sbKey, serviceUri, expiry);
            */

            var client = EventHubClient.CreateFromConnectionString("Endpoint=sb://bbvaeventhub-ns.servicebus.windows.net/;SharedAccessKeyName=winformApp;SharedAccessKey=OuUNs2AMtnOdZwgiXHrVk3k8KPAjWIZ6EkGr3Nnc1io=", "bbvaeventhub");


            try {

                var serializedString = JsonConvert.SerializeObject(mensaje);
                //var data = new EventData(Encoding.Unicode.GetBytes(serializedString));
                var data = new EventData(Encoding.UTF8.GetBytes(serializedString));
                

                client.Send(data);
            } catch (Exception exp) {
                logOperation("Error on send: ", exp.Message);
            }

        }



        private string getTTMLFullText(XmlDocument ttml) {
            string result = "";

            XmlNode root = ttml.DocumentElement;

            XmlNodeList xnl = ttml.SelectNodes("//P");
            foreach (XmlNode xn in xnl) {
                result += xn.InnerText + " ";
            }
            return result;
        }



        private void rellenaListaFicheros() {
            listFiles.Items.Clear();
            foreach (string fich in _listaFicheros)
                listFiles.Items.Add(fich);
        }





        delegate void logOperationDelegate(string operation, string text);
        private void logOperation(string operation, string text) {

            if (!InvokeRequired) {
                textLog.Text += System.DateTime.Now + " - " + operation + System.Environment.NewLine;
                textLog.Text += text;
                textLog.Text += System.Environment.NewLine;
                textLog.Text += System.Environment.NewLine;
            } else {
                // Show progress asynchronously
                logOperationDelegate logOp = new logOperationDelegate(logOperation);
                this.Invoke(logOp, new object[] { operation, text });
            }

        }








        #endregion
 
    }
}
