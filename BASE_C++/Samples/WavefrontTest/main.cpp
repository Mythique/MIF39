#include "QImageLoader/qimageloader.hpp"
#include "Wavefront/wavefront.hpp"
#include "ServerManager/ServerManager.hpp"

#include <iostream>
#include <string>
#include <sstream>

extern void __attach(void);
extern void __attachInterfaces(void);
extern void __attachGenerics(void);
extern void __attachAssets(void);
extern void __attachQImage(void);
extern void __attachWavefront(void);

void writeToFile(EncByteBuffer& b, std::string name)
{
   unsigned char* data=b.getData();

    std::cout << std::endl;

    std::cout << "\t J'écris le buffer : " << std::endl << data << std::endl;

    std::string filename = "Data" + name + ".bin";
    FILE* file = fopen( filename.c_str(), "wb" );

    for(int i = 0; i < b.getLength(); i++)
        {
            char c=data[i];
            fprintf(file, "%c",c);
        }
    fclose(file);
}

int main(int argc, char *argv[])
{
    /*
    __attach();
    __attachInterfaces();
    __attachGenerics();
    __attachAssets();*/
    __attachQImage();
    __attachWavefront();
    FileDescriptor file ( argv[1]);
    
    QList < SharedResourcePtr > ress = ResourceHolder::Load(file);
    /*
     * ByteBuffer tmp = ResourceHolder::ToBuffer(ress[0]);
    for ( unsigned int i = 0 ; i < tmp.getLength() ; i ++ ) {
        printf ( "%02x:", tmp.getData() [ i ] );
        if ( ( i % 16 ) == 0 ) printf ( "\n" );
    }*/

    int i = 1;
    foreach ( QUuid id, ResourceHolder::AllKeys() ) {
        std::stringstream ss;
        ss << i;
        SharedResourcePtr res = ResourceHolder::GetByUUID(id);
        res->Usage();
        EncByteBuffer buffer = ResourceHolder::ToBuffer( res );
        writeToFile(buffer, ss.str());
        i++;
    }
/*

    ResourceHolder::Load(file);
ResourceHolder::Usage();
    int i = 1;
    foreach ( QUuid id, ResourceHolder::AllKeys() ) {
        std::stringstream ss;
        ss << i;
        SharedResourcePtr res = ResourceHolder::GetByUUID(id);
        res->Usage();
        EncByteBuffer buffer = ResourceHolder::ToBuffer( res );
        ServerManager::getInstance()->writeToFile(buffer, ss.str());
        unsigned long long index = 0;
        SharedResourcePtr ptr = ResourceHolder::FromBuffer(buffer,index);
        ptr->Usage();
        i++;
    }*/
    return 0;
}
