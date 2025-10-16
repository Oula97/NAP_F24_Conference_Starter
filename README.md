# NAP F24 Conference Application

**Course:** Network Application Programming (NAP F24)  
**Instructor:** Eng. Ghayth Moustapha  
**Deadline:** 25-10-2025  

---

## **Project Overview**
This project is a real-time conference application developed in **C# / .NET Core**.  
It includes:
- **Text Chat** using TCP  
- **Audio/Video Streaming** using UDP  

The application supports multiple participants in conference rooms with real-time communication.

---

## **Project Structure**

### **Server**
- **NAP_F24_ConferenceApp_Server**
  - `Program.cs` → Initializes TCP and UDP servers  
  - `RoomManager.cs` → Manages rooms and participants  
  - `TcpChatHandler.cs` → Handles TCP chat connections  
  - `UdpStreamHandler.cs` → Handles UDP video/audio streaming  

### **Client**
- **NAP_F24_ConferenceApp_Client**
  - `MainDashboard.cs` → Main UI to create/join rooms  
  - `RoomForm.cs` → Chat, audio/video streaming interface  
  - `TcpChatClient.cs` → TCP client wrapper for chat  
  - `UdpStreamClient.cs` → UDP client wrapper for video/audio  
  - `UdpVideoAudioManager.cs` → Sends video/audio via UDP  

---

## **Setup Instructions**

### **1. Server**
1. Open `NAP_F24_ConferenceApp_Server` in Visual Studio.  
2. Restore NuGet packages if needed.  
3. Build and run the server.  
4. The server will start:  
   - **TCP Port:** 5000 (Chat)  
   - **UDP Port:** 6000 (Video) and 6001 (Audio)  

**Note:** Server console will display connected clients and messages.

---

### **2. Client**
1. Open `NAP_F24_ConferenceApp_Client` in Visual Studio.  
2. Restore NuGet packages:  
   - `AForge.Video` / `AForge.Video.DirectShow` (for video capture)  
   - `NAudio` (for audio capture/playback)  
3. Build and run the client.  
4. In the **Start** form, click to open **MainDashboard**.  
5. Create a new room or join an existing room.  
6. In **RoomForm**:  
   - Chat via TCP in real-time  
   - Video streaming via webcam  
   - Audio streaming via microphone  
   - Control buttons: Mute/Unmute, Stop/Start Video  

---

### **3. Notes**
- Default server address: `127.0.0.1`  
- Default TCP/UDP ports must match server configuration.  
- Multiple clients can join the same room for real-time communication.  
- Leaving a room cleans up resources automatically.  

---

### **4. Features**
- Multi-room management  
- Real-time TCP chat between participants  
- Real-time UDP video and audio streaming  
- GUI for chat, video, audio controls  
- Multithreaded server handling multiple clients concurrently  

---

### **5. Future Improvements**
- Synchronize client room selection with server-side RoomManager  
- Use `async/await` for UDP receiving for better scalability  
- Automatic reconnection on network failure  
- Display real-time participant count per room  

---

### **6. Testing**
1. Start server.  
2. Launch multiple clients.  
3. Join the same room from multiple clients.  
4. Test sending chat messages and ensure all participants receive them.  
5. Test video/audio streaming: ensure frames and audio are visible/audible to others.  

---

### **7. Changelog** Change log.md
- **v1.0** (15-10-2025): Initial release with TCP chat, UDP video/audio streaming, multi-room support, and GUI.  

---

