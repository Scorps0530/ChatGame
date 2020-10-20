var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(5432);
console.log('10.61.37.4:5432 서버 실행됨.')

io.on('connection', function(socket){
    console.log('클라이언트 접속')
    console.log(socket.request.connection.remoteAddress)
	socket.on('beep', function(data){
        console.log('beep 이벤트 발생')
        console.log(data);                              // 클라이언트로부터 받은 데이터		 -> { msg: 'Hi server!!!' }
        console.log(data.msg)															// -> Hi server!!!
		socket.emit('boop', {msg: 'Hello client'});     // 클라이언트로 데이터 전송 
	});
})
// npm i nodemon -g
// nodemon -> 자동으로 서버를 리스타트 시켜준다
// 실행 : nodemon index.js