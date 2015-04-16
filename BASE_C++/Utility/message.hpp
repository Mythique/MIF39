#ifndef MESSAGE_HPP
#define MESSAGE_HPP
#include "bytebuffer.hpp"
#include <stdio.h>

class EncByteBuffer : public ByteBuffer{
private :
    int type;

public:
    int getType() const {return type;}
    void setType(int a){type = a;}

    /*inline unsigned long long getLength () { return data.getLength(); }
    inline const unsigned long long getLength () const { return data.getLength(); }
    unsigned char * getDataOnly(){ return(data.getData());}

    inline unsigned char* getData (){
        /*unsigned char* msg = new unsigned char[data.getLength() + 4];
        msg[0] = this->type>>24;
        msg[1] = this->type>>16;
        msg[2] = this->type>>8;
        msg[3] = this->type;
        strcpy((char *)msg + 4, (const char*)this->data.getData());
        return msg;
        return data.getData();
    }

    inline const unsigned char* getData() const{
        return data.getData();
    }*/

    EncByteBuffer(){
        type = -1;
    }

    EncByteBuffer(unsigned char * msg, unsigned long long length) : ByteBuffer(msg, length), type(-1){
    }

    EncByteBuffer(int t, ByteBuffer data) : ByteBuffer(data), type(t){
    }

    EncByteBuffer(ByteBuffer buff) : ByteBuffer(buff), type(-1){
    }

};
#endif
