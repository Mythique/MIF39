#ifndef SERVEURINTER_H
#define SERVEURINTER_H

#include <iostream>
#include "AssetInterfaces/resourceholder.hpp"
#include <Utility/message.hpp>
//#include "TcpNetworking/simpletcpstartpoint.hpp"
#include "ServerManager/EncTcpStartPoint.hpp"

class ServerManager
{
public :
    enum dataType {COMPRESSED=1, SHARED_R=2, ID_CHUNK=4, ID_OBJ=8, ID_ASSET=16, ID_THINGIE=32, TEST = -1};
    void writeToFile(EncByteBuffer& b);
    static ServerManager* getInstance();
    QList<QUuid> getListIDObj(QUuid);
    SharedResourcePtr getAsset(QUuid);
    QList<QUuid> getListIDThings(QUuid);
    //bool configure(SimpleTcpStartPoint::Options& option);
    bool linkTcp(EncTcpStartPoint& tcp);
    bool startConnection();
    bool stopConnection();
    EncTcpStartPoint* getConnection();
    bool interpret(QUuid);



protected :
    static bool written;
    static ServerManager* instance;
    EncTcpStartPoint* connection;

private:
    ServerManager();



};

#endif // SERVEURINTER_H
