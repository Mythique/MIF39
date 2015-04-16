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
    if(connection != 0)
        stopConnection();
    connection = &tcp;
    return true;
}

void ServerManager::writeToFile(EncByteBuffer& b)
{
   unsigned char* data=b.getData();
    if(written)
        return;

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
    SharedResourcePtr reponse;
    //Commenté en attente de la correction de ResourceHolder
    //res= ResourceHolder::GetByUUID(q);
    return reponse;
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
    if(connection != 0){
        connection->stop();
        return true;
    }
    return false;
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
    }

    if(!connection->isStarted()){

        std::cout << "erreur : connection non démarrée." << std::endl;
        return false;
    }

    std::cout << "Client OK" << std::endl;

    EncByteBuffer requete, reponse;
    requete.setType(-2);
    SharedResourcePtr res;
    QUuid req;
    long long unsigned int l;

    while(requete.getType() == -2) {
        if(connection->isStarted()){
            std::cout << "EncTcpStartPoint try receiving" << std::endl;
            connection->receive(client, requete);
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
                ::fromBuffer(requete, l, req);
                //res = ServerManager::getInstance()->getRessource(req);
                reponse.setType(SHARED_R);
                reponse.append(res->convertToBuffer());
                connection->send(client, reponse);
                break;


            case ServerManager::TEST :
                std::cout << "Received TEST" << std::endl;
            	reponse.setType(TEST);
                l = requete.getLength();
                ::fromBuffer(requete, l, req);
                //res = ServerManager::getInstance()->getRessource(req);
                QUuid q;
                reponse.append(ByteBuffer((unsigned char*)(q.toByteArray().data()), q.toByteArray().size()));
                connection->send(client, reponse);
            	std::cout << "Test reçu" <<std::endl;
            	break;
        }

    return true;
}
