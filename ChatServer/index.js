var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(4567);
console.log('127.0.0.1:4567 서버 실행됨.')

io.on('connection', function(socket){
    console.log('클라이언트 접속')
    console.log(socket.request.connection.remoteAddress)

    // 새로운 채팅 메시지가 수신되면 모두에게 전송
    socket.on('newMsg', (data)=>{
        console.log('newMsg 이벤트 발생 : ', data);      // 클라이언트로부터 받은 데이터
        io.emit('broadcastMsg', {msg: data.msg})     // 모든 클라이언트에게 메시지 전송
    })

    // 새로운 사용자 접속 이벤트
    socket.on('joinNewUser', (data)=>{
        socket.userName = data.userName;
        console.log('joinNewUser 이벤트 발생 : ', data);      // 클라이언트로부터 받은 데이터
        io.emit('joinNewUser', {userName: data.userName})     // 모든 클라이언트에게 메시지 전송
    })

    // 클라이언트 접속 해지 이벤트
    socket.on('disconnect', (reason)=>{
        console.log(`${socket.userName} 접속 해제`)
        console.log('reason', reason);
        io.emit('disconnetUser', {userName: socket.userName})
    })
})