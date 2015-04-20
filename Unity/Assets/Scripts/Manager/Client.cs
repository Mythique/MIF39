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
public sealed class Client
{	

	public string ip="192.168.1.116";
public int port=3000;
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
	private TcpClient tcpclnt;

	/*
	 * Méthode de demande de ressource au serveur
	 * @param : int (type de la demande), Guid (uid de la ressource demandée)
	 * @return : void
	 */
	public void ask(ServerAnswerManager.Type type,Guid param)
	{
		Connect (ip, port);
		bool read = false;
		int msgSize;
		Stream stm = tcpclnt.GetStream();
		byte[] t = BitConverter.GetBytes ((int)type);
		stm.Write (t, 0, t.Length);
		stm.Write (BitConverter.GetBytes(38), 0, 4);
		byte[] guid = System.Text.Encoding.ASCII.GetBytes ("{"+param.ToString()+"}");
		Debug.Log (System.Text.Encoding.ASCII.GetString(guid));
		stm.Write(guid, 0, guid.Length);
		stm.Flush ();
		ServerAnswerManager.getInstance ().addContents (stm);
		Disconnect();
	}

	/*
	 * Méthode de connexion au serveur
	 * @param : string (adresse IP du serveur), int (numéro de port à atteindre)
	 * @return : void
	 */
	public void Connect(string ipAdresse, int port)
	{
		try 
		{
			tcpclnt = new TcpClient();
			Logger.Trace("Connecting.....");

			if(ipAdresse != "")
			{
				Debug.Log(ipAdresse);
				tcpclnt.Connect(ipAdresse,port);
			}
			else
			{
				tcpclnt.Connect(IPAddress.Any,port);
			}

			Logger.Trace("Addresse okok");
		}
		
		catch (Exception e)
		{
			Logger.Error("Error..... " + e.StackTrace);
		}
	}

	/*
	 * Méthode de déconnexion au serveur
	 * @param :
	 * @return : void
	 */
	public void Disconnect() {
		try 
		{
			tcpclnt.Close();
		}
		catch(Exception e)
		{
			Logger.Error("Error disconnecting : " + e.ToString());
		}
	}

}
