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
 * @param ID d'une cellule
 * return Liste des IDs des Assets contenus dans la cellule
 */
QList<QUuid> ServerManager::getListIDAsset(QUuid){
    QList<QUuid> list;
    return list;
}

/*
 * @param ID d'un Asset
 * return liste des IDs des objets liés à l'Asset
 */
QList<QUuid> ServerManager::getListIDObj(QUuid){
    QList<QUuid> list;
    return list;
}

QList<QUuid> ServerManager::getListIDThings(QUuid){
    QList<QUuid> list;
    return list;
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
    QList<QUuid> list;
    QUuid req;
    SharedResourcePtr shared;
    ByteBuffer resource;
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
                std::cout << "Received SHARED_R" << std::endl;
                l = requete.getLength();
                ::fromBuffer(requete, l, req);
                reponse.setType(SHARED_R);
                shared = ResourceHolder::GetByUUID(req);
                resource = ResourceHolder::ToBuffer(shared);
                reponse.append(resource);
                connection->send(client, reponse);
                break;

            case ServerManager::ID_CHUNK :
                std::cout << "Received ID_CHUNK" << std::endl;
                l = requete.getLength();
                ::fromBuffer(requete, l, req);
                list = ServerManager::getInstance()->getListIDObj(req);
                reponse.setType(ID_OBJ);
                for(int i = 0; i < list.size(); ++i) {

                    ByteBuffer tmp((unsigned char*)(list.at(i).toByteArray().data()), list.at(i).toByteArray().size());
                    reponse.append(tmp);
                }
                connection->send(client, reponse);
                break;

            case ServerManager::ID_OBJ :
                std::cout << "Received ID_OBJ" << std::endl;
                l = requete.getLength();
                ::fromBuffer(requete, l, req);
                list = ServerManager::getInstance()->getListIDThings(req);
                reponse.setType(ID_THINGIE);
                for(int i = 0; i < list.size(); ++i) {

                    ByteBuffer tmp((unsigned char*)(list.at(i).toByteArray().data()), list.at(i).toByteArray().size());
                    reponse.append(tmp);
                }
                connection->send(client, reponse);
                break;
            case ServerManager::TEST :
                std::cout << "Received TEST" << std::endl;
            	reponse.setType(TEST);
                /*bool t = true;
                ByteBuffer tmp((unsigned char*)t, sizeof(bool));
                std::cout << "Appending TEST" << std::endl;
                reponse.append(tmp);
                std::cout << "Sending TEST" << std::endl;*/
                l = requete.getLength();
                ::fromBuffer(requete, l, req);
                list = ServerManager::getInstance()->getListIDThings(req);
                QUuid q;
                ByteBuffer tmp((unsigned char*)(q.toByteArray().data()), q.toByteArray().size());
                reponse.append(tmp);
                connection->send(client, reponse);
            	std::cout << "Test reçu" <<std::endl;
            	break;
        }

    return true;
}
