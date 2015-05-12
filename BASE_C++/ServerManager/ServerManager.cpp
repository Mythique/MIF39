#include "ServerManager.hpp"
#include <typeinfo>
#include <unistd.h>

//SimpleTcpStartPoint::Options options(3000,1);
ServerManager* ServerManager::instance = new ServerManager();
bool ServerManager::written = false;

/*ServerManager::ServerManager(SimpleTcpStartPoint::Options& options) : connection(options)
{
    std::cout << "instanciation unique de servermanager" << std::endl;

}//*/

ServerManager::ServerManager() {
    std::cout << "ServerManager instancié v5" << std::endl;
}

bool ServerManager::linkTcp(EncTcpStartPoint& tcp) {
    std::cout << "link avant verification " << std::endl;
    if(connection == 0)
        while(!stopConnection()){}
    std::cout << "link apres verification " << std::endl;
    connection = &tcp;
    return true;
}

void ServerManager::writeToFile(EncByteBuffer& b)
{
   unsigned char* data=b.getData();
    //if(written)
        //return;

    std::cout << std::endl;

    std::cout << "\t J'écris le buffer : " << std::endl << data << std::endl;

    //*
    char* filename = "Data.txt";
    FILE* file = fopen( filename, "wb" );

    for(int i = 0; i < b.getLength(); i++)
        {
            char c=data[i];
            fprintf(file, "%c",c);
        }
    fclose(file);
    //*/
    written = true;
}

ServerManager *ServerManager::getInstance()
{
    return instance;
}




/*
 * Fonction renvoyant une ressource correspondant à l'ID passé en paramètre
 * @param : QUuid
 * @return : SharedResourcePtr
 */
SharedResourcePtr ServerManager::getRessource(QUuid q){
    SharedResourcePtr res;
    //Commenté en attente de la correction de ResourceHolder
    res= ResourceHolder::GetByUUID(q);
    return res;
}

bool ServerManager::startConnection()
{
    if(connection != 0) {
        connection->start();
        return true;
    }
    return false;
}

bool ServerManager::stopConnection()
{
    bool res;
    std::cout << "stop avant verification " << std::endl;
    if(connection != 0){
        std::cout << "stop apres verification " << std::endl;
        res = connection->stop();
        std::cout << "apres stop" << std::endl;
    }
    return res;
}

EncTcpStartPoint *ServerManager::getConnection()
{
    return connection;
}

bool ServerManager::interpret(QUuid client){

    if(client == NULL){

        std::cout << "erreur : client incorrect." << std::endl;
        return false;
    }
    if(connection == 0) {
        std::cout << "Error : no socket bound" << std::endl;
        return false;
    }

    if(!connection->isStarted()){

        std::cout << "erreur : connection non démarrée." << std::endl;
        return false;
    }

    std::cout << "Client OK" << std::endl;

    EncByteBuffer requete, reponse;
    requete.setType(-2);
    SharedResourcePtr res;
    QUuid req, q;
    EncByteBuffer resource;
    long long unsigned int l;

    while(requete.getType() == -2) {

        if(connection->isStarted()){
            std::cout << "EncTcpStartPoint try receiving" << std::endl;
            
			if(! connection->receive(client, requete) ) {
				std::cout << "Not able to receive" <<std::endl;
				return false;
			}
            std::cout << "After receiving " << requete.getType() << "/" << requete.getLength() << std::endl;
        }
        else
        {
            std::cout << "Connection dropped" << std::endl;
            return false;

        }
    }
        std::cout << "Receiving request" << std::endl;
        std::cout << "Type " << requete.getType() << " , length " << requete.getLength() << std::endl;
        
		switch(requete.getType()){

            case ServerManager::SHARED_R :
                std::cout << "Received Request for resource" << std::endl;
                l = requete.getLength();
                req = QUuid((const char*)(requete.getData()));
                res = ServerManager::getInstance()->getRessource(req);
                reponse.setType(SHARED_R);
                resource = ResourceHolder::ToBuffer(res);
                reponse.append(resource);
                break;

            case ServerManager::TEST :
                std::cout << "Received TEST" << std::endl;
            	reponse.setType(TEST);
                l = requete.getLength();
                ::fromBuffer(requete, l, req);
                res = ServerManager::getInstance()->getRessource(req);

                reponse.append(ByteBuffer((unsigned char*)(q.toByteArray().data()), q.toByteArray().size()));
                break;

			case ServerManager::WORLD :
				std::cout << "Error : Unexpected type WORLD." << std::endl;
				return false;
				break;

			default :
				std::cout << "Error : Unknown type " << requete.getType() << std::endl;
				return false;
				break;
        }

		if(! connection->send(client, reponse )){
				std::cout << "Couldn't send" << std::endl;
				return false;
		}

    return true;
}
