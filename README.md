# Phoenix Networking

TCP/UDP generic networking package that can be used in Unity projects.

Early project. Under development. Public contributions are welcome. Contact me at [LinkedIn/Bezmican Zehir](https://www.linkedin.com/in/bezmicanzehir/) for details

---

### Network Communications and Event Diagrams;
[ServerTick <-> ClientInputTick](https://github.com/kirisakiken/phoenix-networking/blob/master/.docs/PhoenixNetworking/UDP/UDP_ClientInputTick_ServerTick_Diagram.png) 

---

### Server Architecture

```
                                                                                          
                                                          +---------------------------------------------------------+
                                    OnClientConnected     | Sends initial connection payload to connected client    |
                                +-----------------------> | Broadcasts connected client packet to available clients |
                                |                         +---------------------------------------------------------+
                                |
+------------------+            |
|                  |            |                         +---------------------------------------------------------+
|                  |    TCP     |   OnClientDisconnected  |   Under development                                     |
|      Server      +---------->+|-----------------------> |                                                         |
|                  |            |                         +---------------------------------------------------------+
|                  |            |
+--------+---------+            |
         |                      |                        +----------------------------------------------------------+
         |                      |   OnMessageReceived    | Broadcasts received message from client to               |
         |                      |   FromClient           | all available clients                                    |
         |                      +----------------------->|                                                          |
         |                                               +----------------------------------------------------------+
         |
         |
         |                     +---------------------------------------------------------+
         |        UDP          |   Broadcasts all added, modified and removed            |
         +-------------------->+   entities to available UDP Clients                     |
                               |                                                         |
                               +---------------------------------------------------------+
```
