#include "EncTcpStartPoint.hpp"
#include "Utility/message.hpp"

EncTcpStartPoint::EncTcpStartPoint(ConnectionStartPoint::Options& options): ConnectionStream< TcpSocket> (options), ConnectionStartPoint < TcpSocket > (options), SimpleTcpStartPoint(options) { }

bool EncTcpStartPoint::send(QUuid client, const EncByteBuffer& buffer) {
    bool result = true;
    int type = buffer.getType();
    unsigned int lg;
    unsigned int length = buffer.getLength();
    lg = mSocket->sendData( client, (unsigned char*) & type, sizeof(int));
    if(lg != sizeof (int)) {
        std::cout << "Wrong type" << std::endl;
        result = false;
    }
    else {
        lg = mSocket->sendData( client, (unsigned char*) & length, sizeof(unsigned int) );
        if(lg != sizeof(unsigned int)) {
            std::cout << "Wrong length" << std::endl;
            result = false;
        }
        else {
            lg = mSocket->sendData(client, buffer.getData(), buffer.getLength() );
            if (lg != buffer.getLength()) {
                std::cout << "Wrong data" << std::endl;
                result = false;
            }
        }
    }
    mSocket->cleanUp(mStartPointOptions.cbDisconnect);
    return result;
}

bool EncTcpStartPoint::receive(QUuid client, EncByteBuffer& buffer) {
    //std::cout << "EncTcpStartPoint receive" << std::endl;
    bool result = true;
    int type;
    unsigned int lg;
    unsigned int length;
    lg = mSocket->receiveData(client, (unsigned char*) & type, sizeof(int));
    if (lg != sizeof(int)) {
        std::cout << "invalid type" << std::endl;
        result = false;
    }
    else {
        //std::cout << "type received : " << type << std::endl;
        buffer.setType(type);
        lg = mSocket->receiveData(client, (unsigned char*) & length, sizeof(unsigned int));
        if(lg != sizeof(unsigned int)) {
            std::cout << "invalid length" << std::endl;
            result = false;
        }
        else {
            //std::cout << "length received : " << length << std::endl;
            buffer.reserve(length);
            lg = mSocket->receiveData(client, buffer.getData(), buffer.getLength());
                    if(lg != buffer.getLength()) {
                        std::cout << "invalid data" << std::endl;
                        result = false;
                    }
                    else {
                        //std::cout << "data got" << std::endl;
                    }
        }
    }
    mSocket->cleanUp(mStartPointOptions.cbDisconnect);
    return result ;
}
