import socket
import logging

from time import gmtime,strftime
LOGFILENAME = strftime("%Y-%m-%d", gmtime())+"pi.log"

logging.basicConfig(filename=LOGFILENAME,level=logging.DEBUG,format='%(asctime)s %(levelname)s:%(message)s')
from config import *
logging.debug("Reading configs file")


while 1:
    MESSAGE=input("Data:")
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    ENCODED_MSG = (SERIAL +":"+MESSAGE).encode()
    logging.info("Sent msg to server : "+SERIAL+":"+MESSAGE)
    s.send(ENCODED_MSG)
    
    data = s.recv(BUFFER_SIZE)
    print (data.decode())
    s.close()
    if not data: break
