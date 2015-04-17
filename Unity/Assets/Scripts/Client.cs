using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

/*
 * Class Client
 * représentant un client de connection au serveur
 * class implémentant un patern Singleton thread safe
 * Il ne peut etre créé qu'une seule instance de cette classe à la fois
 */
public sealed class Client: MonoBehaviour
{	
	/*
	 * instanciation unique de la classe
	 */
	private static readonly Client instance = new Client();

	/*
	 * Constructeur privé (à ne pas essayer d'appeler)
	 */
	static Client (){
	}

	/*
	 * Méthode de récupération de l'instance unique
	 * @param : 
	 * @return : Client (instance du client)
	 */
	public static Client getInstance(){

		return instance;
	}

	/*
	 * attribut privé contenant un client tcp pour permettre la connection
	 */
	private static TcpClient tcpclnt;

	/*
	 * Méthode de demande de ressource au serveur
	 * @param : int (type de la demande), Guid (uid de la ressource demandée)
	 * @return : void
	 */
	public void ask(int type,Guid param)
	{
		bool read = false;
		int msgSize;
		Stream stm = tcpclnt.GetStream();
		byte[] t = BitConverter.GetBytes (type);
		stm.Write (t, 0, t.Length);
		stm.Write (BitConverter.GetBytes(38), 0, 4);
		byte[] guid = System.Text.Encoding.ASCII.GetBytes ("{"+param.ToString()+"}");
		Debug.Log (System.Text.Encoding.ASCII.GetString(guid));
		stm.Write(guid, 0, guid.Length);
		stm.Flush ();
		ServerAnswerManager.getInstance ().addContents (stm);
	}

	/*
	 * Méthode de connexion au serveur
	 * @param : string (adresse IP du serveur), int (numéro de port à atteindre)
	 * @return : void
	 */
	public static void Connect(string ipAdresse, int port)
	{
		try 
		{
			tcpclnt = new TcpClient();
			Debug.Log("Connecting.....");

			if(ipAdresse != "")
			{
				Debug.Log(ipAdresse);
				tcpclnt.Connect(ipAdresse,port);
			}
			else
			{
				tcpclnt.Connect(IPAddress.Any,port);
			}

			Debug.Log("Addresse okok");
		}
		
		catch (Exception e)
		{
			Debug.Log("Error..... " + e.StackTrace);
		}
	}

	/*
	 * Méthode de déconnexion au serveur
	 * @param :
	 * @return : void
	 */
	public static void Disconnect() {
		try 
		{
			tcpclnt.Close();
		}
		catch(Exception e)
		{
			Debug.Log("Error disconnecting : " + e.ToString());
		}
	}

	/*
	 * Méthode de lancement du client
	 * @param :
	 * @return : void
	 */
	void Start(){
		Connect("192.168.1.168", 3000);

		//Guid testGuid = new Guid ("60633b7c-3d7e-d298-384a-22a37ecc723a");
		//Guid testGuid = new Guid ("2c8f0939-2be1-0b49-0178-8e89bd4f6986");
		//Guid testGuid = new Guid ("d45925f5-9829-fe00-cf43-13b6ef24e539");

		//int testType = (int)ServerAnswerManager.Type.SHARED_R;

		//bool continued = true;
		//ask(testType, testGuid);
		//Disconnect ();

	}

	/*
	 * Méthode de mise à jour du RessourceLoader
	 */
	void Update(){
		ResourceLoader.getInstance().load ();
	}

}
