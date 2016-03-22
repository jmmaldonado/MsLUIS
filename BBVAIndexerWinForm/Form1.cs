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
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace BBVAIndexerWinForm {




    public partial class Form1 : Form {


        private List<String> _listaFicheros;
        XmlDocument XMLDoc;

        List<Uri> indexedFilesList;
        string _LUISQuery;
        int _LUISMaxCharacters;
        bool _verbose;
        bool _envioActivado;

        List<string> diccionarioFinanciero;
        List<string> palabrasInutiles;


        


        public Form1() {
            InitializeComponent();
            _listaFicheros = new List<string>();
            _listaFicheros.Clear();

            _LUISQuery = "https://api.projectoxford.ai/luis/v1/application?id=" + Properties.Settings.Default.LUISAppID + "&subscription-key=" + Properties.Settings.Default.LUISKey + "&q=";

            XMLDoc = new XmlDocument();
            _LUISMaxCharacters = Properties.Settings.Default.LUISMaxCharacters;
            _verbose = Properties.Settings.Default.VerboseLogging;
            _envioActivado = Properties.Settings.Default.activarEnviosAServiceBus;
            //XDoc = new XDocument();

            //Preparacion y carga del diccionario financiero
            updateStatus("Loading financial dictionary");
            diccionarioFinanciero = prepararDiccionario(Properties.Settings.Default.diccionarioFinancieroPath);
            palabrasInutiles = prepararDiccionario(Properties.Settings.Default.palabrasInutilesPath);
            logOperation("Finalizada la carga del diccionario financiero", "El diccionario recoge " + diccionarioFinanciero.Count + " terminos financieros");
            updateStatus("Finished loading financial dictionary");

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

            //textLog.Text = getQuerySentiment("Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituales el perfil por lo que la transferencia se realizado que pase un buen día.  Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituasdfsdfsdfsdfsdfles el perfil por lo que la transferencia se realizado que pase un buen día. Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituales el perfil por lo que la transferencia se realizado que pase un buen día.  Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituasdfsdfsdfsdfsdfles el perfil por lo que la transferencia se realizado que pase un buen día. Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituales el perfil por lo que la transferencia se realizado que pase un buen día.  Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituasdfsdfsdfsdfsdfles el perfil por lo que la transferencia se realizado que pase un buen día. Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituales el perfil por lo que la transferencia se realizado que pase un buen día.  Hola buenos días mi nombre es agente del banco en qué puedo ayudarle buenos días llamaba para hacerlo transferencia monetaria mi primo luis. Por supuesto podría estar confírmame su número de neil. Claro es el cinco ocho siete seis siete tres cuatro cinco letras q. Qué cantidad le gustaría transferir cuatrocientos veintisiete euros con cincuenta céntimos. Que confirmó que su primo luis aparece registrado como contacto habituasdfsdfsdfsdfsdfles el perfil por lo que la transferencia se realizado que pase un buen día. ").ToString();
            string original = "Bueno el caso en el bancoen quÃ© puedo ayudarle buenos dÃ­as tengan demonio querÃ­a ponerme en contacto con el banco me llamado kozmo llamaba para hablar sobre un prÃ©stamo para una vivienda un momento por favordisponible si muchas gracias. LeuniÃ³n fin informaciÃ³nsobre un beso para una vivienda. Llegar. Entendido SeÃ±oray enseÃ±o lotener en su momento me podia da su nÃºmero de tel Ã©fono para que se ponga en contacto cuando le sea posible por supuestocincuenta y cuatro setecientos noventa y tres . novecientos veintidÃ³s le confirmo seiscientos cincuentasetecientos noventa y tres novecientos veintidÃ³s y congracia que pase un buen SÃ­ dÃ­game malasia se. Y buenos dÃ­as soyvendo. Interesada en saber cÃ³mo para comprar una vivienda eninteresado me pidiÃ³ dis cretamente debido a que estÃ¡ interesada en uno de su en localizarlo por internet ademÃ¡s querÃ­a saber cuales son la posibilidad de adquirir un prÃ©stamo para conseguir si le de laÃºltima nÃ³mina una baliza siemprecuando lasaÃ±a no suben el treinta y cinco por ciento de la depresiÃ³n la vivienda y por Ãºltima el tener un banco y acuerdo. Y si entregase cincuenta porde la vivienda noningÃº n tipo de ventaja hablando de la unidad econÃ³mica importante quÃ© le parece maÃ±ana con mi oficina sobre la de la cuerda perfecto maÃ±ana entonces gracias por su atenciÃ³n graciasun dÃ­a.  ";
            string resultado = cleanUselessWordsFromQuery(original);


            resultado = Regex.Replace(original, "\\b" + String.Join("\\b|\\b", palabrasInutiles.ToArray()) + "\\b", "");


            textLog.Text = "ORIGINAL " + original;
            textLog.Text += Environment.NewLine + Environment.NewLine + resultado;
        }









        

        


        


















        ///////////////////////////////////////////////////////////
        #endregion












































        private void botAuto_Click(object sender, EventArgs e) {
            pruebaLlamadaJSON();
        }






        #region CODIGO ACABADO





        /// <summary>
        /// Procesa la query contra el diccionario financiero, devuelve un string con todos los terminos financieros encontrados en la query
        /// </summary>
        /// <param name="query">Texto en el que buscar términos financieros</param>
        /// <returns>String con todos los terminos financieros encontrados</returns>
        private string getFinancialKeywordsFromQuery(string query) {
            try {
                updateStatus("Obtaining financial keywords from query");
                string result = "";
                string[] temp = query.Split(' ');
                List<string> lista = new List<string>();

                //Busqueda de cada uno de los términos de la query contra el diccionario financiero
                foreach (string palabra in temp)
                    if (diccionarioFinanciero.Contains(palabra))
                        lista.Add(palabra);

                //Aplanado de la lista de keywords encontrados a un string separado por espacios
                foreach (string palabra in lista)
                    result += palabra + " ";


                logOperation("Analisis financiero de la query", "Obtenidos " + lista.Count + " terminos financieros en la query");
                if (_verbose) logOperation("[VERBOSE] Keywords financieros encontrados en la query", result);
                updateStatus("Finished obtaining financial keywords from query");

                return result;
            } catch (Exception ex) { logOperation("[ERROR] Error en getFinancialKeywordsFromQuery", ex.Message); return ""; }
        }



        /// <summary>
        /// Elimina de la query las palabras inutiles (preposiciones, articulos, ..., etc
        /// </summary>
        /// <param name="query">Texto del que queremos eliminar las palabras inutiles</param>
        /// <returns>Si hay algún error en la ejecución de este método se devuelve la query original</returns>
        private string cleanUselessWordsFromQuery(string query) {
            try {
                string result = query;

                result = result.Replace("...", " ");
                result = result.Replace(".", "");
                result = result.Replace(",", "");
                result = result.Replace(":", "");
                result = Regex.Replace(result, "\\b" + String.Join("\\b|\\b", palabrasInutiles.ToArray()) + "\\b", "", RegexOptions.IgnoreCase);

                if (_verbose) logOperation("[VERBOSE] Eliminadas las palabras inutiles de la query, esta es la query resultante", result);

                return result;
            } catch (Exception ex) { logOperation("[ERROR] Excepcion en cleanUselessWordsFromQuery", ex.Message); return query; }
        }



        /// <summary>
        /// Procesa el fichero indicado como diccionario financiero. El fichero debe tener un termino por línea (aunque cada término puede tener varias palabras)
        /// </summary>
        /// <param name="path">Path hasta el diccionario financiero</param>
        /// <returns>Lista con los terminos financieros</returns>
        private List<string> prepararDiccionario(string path) {

            List<string> diccionario = new List<string>();
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
                diccionario.Add(line);

            file.Close();

            return diccionario;

        }





        /// <summary>
        /// Hace la llamada a la api de SentimentAnalysis con las credenciales y endpoints definidos en los settings de la aplicacion
        /// En caso de devolver error la llamada a la API se devolvera un score de sentimiento neutral (0.5)
        /// </summary>
        /// <param name="query">Texto sobre el que se quiere analizar el sentimiento</param>
        /// <returns>Score tipo double con el sentimiento del texto, 0.0 = muy malo, 1.0 = muy bueno</returns>
        private double getQuerySentiment(string query) {

            try {

                using (var httpClient = new HttpClient()) {

                    //string inputTextEncoded = HttpUtility.UrlEncode(query);
                    string inputTextEncoded = query;

                    httpClient.BaseAddress = new Uri(Properties.Settings.Default.sentimentAnalysisAPIServiceBaseUri);
                    string creds = "AccountKey:" + Properties.Settings.Default.sentimentAnalysisAPIKey;
                    string authorizationHeader = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(creds));
                    httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // get sentiment
                    string sentimentRequest = Properties.Settings.Default.sentimentAnalysisAPIEndpoint + inputTextEncoded;

                    Task<HttpResponseMessage> responseTask = httpClient.GetAsync(sentimentRequest);
                    responseTask.Wait();
                    HttpResponseMessage response = responseTask.Result;
                    Task<string> contentTask = response.Content.ReadAsStringAsync();
                    contentTask.Wait();
                    string content = contentTask.Result;
                    if (!response.IsSuccessStatusCode) {
                        throw new Exception("Call to get sentiment failed with HTTP status code: " +
                                            response.StatusCode + " and contents: " + content);
                    }

                    SentimentResult sentimentResult = JsonConvert.DeserializeObject<SentimentResult>(content);

                    //Si no hemos obtenido un sentimiento, devolvemos el punto neutro 0.5
                    double result = sentimentResult != null ? sentimentResult.Score : 0.5;

                    if (_verbose) logOperation("[VERBOSE] Resultado del sentimiento analizado", result.ToString());
                    return result;

                }
            } catch (Exception ex) { logOperation("[ERROR] Excepcion en getQuerySentiment", ex.Message.ToString()); return 0.5; }


        }





        /// <summary>
        /// A partir de un objeto LuisResponse generamos un objeto específico para los Intents del evento
        /// De todos los intentes, sólo se enviará el que mayor score haya tenido en el analisis de LUIS
        /// Si se ha configurado la opción removeNONEIntent el intent tipo "None" se filtrará de la respuesta
        /// </summary>
        /// <param name="luis">Objeto tipo LUISResponse que usaremos para rellenar los datos del objeto IntentsPowerBi</param>
        /// <param name="GUID">Identificador unico del evento - generado en esta aplicación - para poder correlar en PowerBi</param>
        /// <param name="dt">Timestamp del evento para poder correlar</param>
        /// <returns></returns>
        private IntentsPowerBi getPowerBIIntentsFromLuisResponse(LUISResponse luis, string GUID, DateTime dt, double sentiment, string cleanQuery, string financialKeywords) {

            IntentsPowerBi result = new IntentsPowerBi();

            Intents tempIntent = new Intents();
            tempIntent.intent = "";
            tempIntent.score = 0;

            //Recorremos todos los intents de la respuesta de LUIS y nos quedamos con la que tenga mayor score
            //Ademas filtramos el intent "None" si se ha especificado en las settings de la aplicación
            foreach (Intents intent in luis.intents) {
                if (!(intent.intent.Equals("None", StringComparison.CurrentCultureIgnoreCase) && Properties.Settings.Default.removeNONEIntent)) {
                    if (intent.score > tempIntent.score) {
                        tempIntent = intent;
                    }
                }
            }

            //Componemos el objeto para enviar a PowerBi
            result.query = luis.query;
            result.queryKeywords = cleanQuery;
            result.intent = tempIntent.intent;
            result.score = tempIntent.score;
            result.sentiment = sentiment;
            result.financialKeywords = financialKeywords;

            result.ProcessedDateTime = dt.ToString();
            result.processedYear = dt.Year;
            result.processedMonth = dt.Month;
            result.processedDay = dt.Day;
            result.processedHour = dt.Hour;
            result.processedMinute = dt.Minute;
            result.processedSecond = dt.Second;
            result.eventGUID = GUID;


            return result;
        }


        private EntitiesPowerBi getPowerBiEntitiesFromLuisResponse(LUISResponse luisResponse, string GUID, DateTime dt, double sentiment, string cleanQuery, string financialKeywords) {
            EntitiesPowerBi result = new EntitiesPowerBi();

            result.query = luisResponse.query;
            result.queryKeywords = cleanQuery;
            result.financialKeywords = financialKeywords;
            result.entities = luisResponse.entities;
            result.sentiment = sentiment;

            result.entity = "JMM";
            result.score = 1;

            result.ProcessedDateTime = dt.ToString();
            result.processedYear = dt.Year;
            result.processedMonth = dt.Month;
            result.processedDay = dt.Day;
            result.processedHour = dt.Hour;
            result.processedMinute = dt.Minute;
            result.processedSecond = dt.Second;
            result.eventGUID = GUID;

            return result;
        }





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

            return result;
        }





        /// <summary>
        /// Invoca a LUIS para obtener el JSON Resultante con los intents y entities de la consulta
        /// Si el parámetro text tiene más de 500 caracteres se encarga de hacer el fragmentado y componer los resultados
        /// </summary>
        /// <param name="text">Texto plano para enviarle a LUIS</param>
        /// <returns>Objeto con el datacontract del JSON resultante de la llamada de Luis, sin duplicados en intents ni entities</returns>
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









        /// <summary>
        /// Es el método principal de la aplicación
        /// 1. Recorre todos los containers de la storage account en busca de los ficheros indexados
        /// 2. Parsea el texto indexado de cada fichero y hace la llamada a LUIS para obtener sus intents y entities
        /// 3. Obtiene el sentimiento del texto indexado
        /// 4. Limpia el texto indexado de palabras inutiles
        /// 5. Busca los términos financieros en el texto indexado
        /// 6. Crea los objetos específicos para la representacion de Intents y Entities en PowerBI
        /// 7. Envia todos los mensajes a los event hubs (respuesta tal cual de LUIS, objeto Intents para PowerBI y Objeto Entities para PowerBI)
        /// </summary>
        private void processIndexedFilesFromStorageAccount() {

            try {

                indexedFilesList = new List<Uri>();
                indexedFilesList.Clear();

                string storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=" + Properties.Settings.Default.IndexedFilesStorageAccountName + ";AccountKey=" + Properties.Settings.Default.IndexedFilesStorageAccountKey;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                IEnumerable<CloudBlobContainer> containerList = blobClient.ListContainers();

                setStatusProgressMaximum(containerList.Count());
                
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
                            updateStatus("Processing " + blob.Uri.Segments.Last() + " file");


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
                                    luisResponse.query = textoBlob; //Volvemos a poner el texto original para evitar el escaping que devuelve LUIS del formato unicode.
                                    
                                    //Obtener el sentimiento de la consulta enviada a LUIS
                                    string sentimentQuery = luisResponse.query.Length > Properties.Settings.Default.sentimentAnalysisAPIMaxQuerySize ? luisResponse.query.Substring(0, Properties.Settings.Default.sentimentAnalysisAPIMaxQuerySize) : luisResponse.query;
                                    double sentiment = getQuerySentiment(sentimentQuery);

                                    //Limpiar la query de palabras Inutiles
                                    string cleanQuery = cleanUselessWordsFromQuery(luisResponse.query);

                                    //Obtener los terminos financieros de la query
                                    string financialKeywords = getFinancialKeywordsFromQuery(luisResponse.query);


                                    //ToDo Separar la respuesta de LUIS en los objetos IntentsPowerBi y EntitiesPowerBi. Cuando esté hecho quitar la llamada a EnviarMensajeAServiceBus
                                    string eventGuid = new Guid().ToString();
                                    DateTime dt = DateTime.Now;
                                    IntentsPowerBi iPBi = getPowerBIIntentsFromLuisResponse(luisResponse, eventGuid, dt, sentiment, cleanQuery, financialKeywords);
                                    EntitiesPowerBi ePBi = getPowerBiEntitiesFromLuisResponse(luisResponse, eventGuid, dt, sentiment, cleanQuery, financialKeywords);


                                    //Envio de los datos a los Service Bus para cada tipo de objeto
                                    if (_envioActivado) {
                                        EnviarMensajeAServiceBus(luisResponse);
                                        EnviarIntentsPowerBiAServiceBus(iPBi);
                                        EnviarEntitiesPowerBiAServiceBus(ePBi);
                                    }
                                }
                            }
    


                        }
                    }
                    updateProgress(1);
                }
                updateStatus("Finished processing indexed files");


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



        private void EnviarIntentsPowerBiAServiceBus(object mensaje) {

            string eventhubForIntentsSASPolicyName = Properties.Settings.Default.eventhubForIntentsSASPolicyName;
            string eventhubForIntents = Properties.Settings.Default.eventhubForIntents;
            string eventhubForIntentsKey = Properties.Settings.Default.eventhubForIntentsKey;
            string serviceBusNamespace = Properties.Settings.Default.servicebusNamespace;

            var client = EventHubClient.CreateFromConnectionString("Endpoint=sb://" + serviceBusNamespace + "/;SharedAccessKeyName=" + eventhubForIntentsSASPolicyName + ";SharedAccessKey=" + eventhubForIntentsKey, eventhubForIntents);


            try {

                var serializedString = JsonConvert.SerializeObject(mensaje);
                var data = new EventData(Encoding.UTF8.GetBytes(serializedString));


                client.Send(data);
            } catch (Exception exp) {
                logOperation("[ERROR] Excepcion en EnviarIntentsPowerBiAServiceBus: ", exp.Message);
            }

        }





        private void EnviarEntitiesPowerBiAServiceBus(object mensaje) {
            string eventhubForIntentsSASPolicyName = Properties.Settings.Default.eventhubForEntitiesSASPolicyName;
            string eventhubForIntents = Properties.Settings.Default.eventhubForEntities;
            string eventhubForIntentsKey = Properties.Settings.Default.eventhubForEntitiesKey;
            string serviceBusNamespace = Properties.Settings.Default.servicebusNamespace;

            var client = EventHubClient.CreateFromConnectionString("Endpoint=sb://" + serviceBusNamespace + "/;SharedAccessKeyName=" + eventhubForIntentsSASPolicyName + ";SharedAccessKey=" + eventhubForIntentsKey, eventhubForIntents);


            try {

                var serializedString = JsonConvert.SerializeObject(mensaje);
                var data = new EventData(Encoding.UTF8.GetBytes(serializedString));


                client.Send(data);
            } catch (Exception exp) {
                logOperation("[ERROR] Excepcion en EnviarEntitiesPowerBiAServiceBus: ", exp.Message);
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




        #endregion





        #region GUI - Delegates y metodos para actualizacion de GUI



        delegate void logOperationDelegate(string operation, string text);
        private void logOperation(string operation, string text) {

            if (!InvokeRequired) {
                //Nuevas entradas del log en la parte superior para facilitar la lectura
                string nuevaLinea = System.DateTime.Now + " - " + operation + System.Environment.NewLine + text + System.Environment.NewLine + System.Environment.NewLine;
                string oldText = textLog.Text;
                textLog.Text = nuevaLinea + oldText;
                /*
                textLog.Text += System.DateTime.Now + " - " + operation + System.Environment.NewLine;
                textLog.Text += text;
                textLog.Text += System.Environment.NewLine;
                textLog.Text += System.Environment.NewLine;
                */
            } else {
                // Show progress asynchronously
                logOperationDelegate logOp = new logOperationDelegate(logOperation);
                this.Invoke(logOp, new object[] { operation, text });
            }

        }




        delegate void updateStatusDelegate(string text);
        private void updateStatus(string text) {

            if (!InvokeRequired) {
                string nuevaLinea = System.DateTime.Now + " - " + text;
                statusLabel.Text = nuevaLinea;
            } else {
                // Show progress asynchronously
                updateStatusDelegate logOp = new updateStatusDelegate(updateStatus);
                this.Invoke(logOp, new object[] { text });
            }

        }






        delegate void updateProgressDelegate(int increment);
        private void updateProgress(int increment) {

            if (!InvokeRequired) {
                int nuevoValor = statusProgress.Value + increment;
                statusProgress.Value = nuevoValor >= statusProgress.Maximum ? statusProgress.Maximum : nuevoValor;
            }
            else {
                updateProgressDelegate upD = new updateProgressDelegate(updateProgress);
                this.Invoke(upD, new object[] { increment });
            }
        }



        delegate void setStatusProgressMaximumDelegate(int max);
        private void setStatusProgressMaximum(int max) {
            if (!InvokeRequired) {
                statusProgress.Maximum = max;
                statusProgress.Value = 0;
            } else {
                setStatusProgressMaximumDelegate spD = new setStatusProgressMaximumDelegate(setStatusProgressMaximum);
                this.Invoke(spD, new object[] { max });
            }
        }









        #endregion


    }
}
