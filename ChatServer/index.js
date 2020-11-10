var io = require('socket.io')({
	transports: ['websocket'],
});
io.attach(4567);  // 포트번호 설정 
console.log('127.0.0.1:4567 서버 실행');

var userName_list=[]              // player 이름 저장 리스트
var gameStrartingUserCount = 2;   // 게임을 시작하는 player 숫자
var requestCount=0;               // requestQuiz 요청 저장 변수
var dap;                          // 정답 임시 저장용 변수

//#region 퀴즈 리스트 준비
var fs = require('fs')
var quizList=[]
fs.readFile('./quiz.txt', 'utf8', function(err, data){
  if(err){
      console.log(err.message)
  }else{
      // 줄바꿈을 기준으로 한 줄씩 quiz와 dap으로 분리하여 quizList에 저장
      data.split('\r\n').forEach(element => {
          quizList.push({
            quiz: element.substr(0, element.indexOf('?')+1), 
            dap: element.trim().substring(element.indexOf('?')+1).trim()
          });
      });

      // 퀴즈 리스트를 콘솔 출력
      quizList.forEach(element=>{
          console.log(element)
      })
      console.log(`퀴즈 ${quizList.length}문제 준비 완료`)
  }
})
//#endregion

io.on('connection', function(socket){
  console.log('클라이언트 접속');
  console.log('New connection from: ', socket.request.connection.remoteAddress);

  // 새로운 사용자 접속 이벤트
  socket.on('joinNewUser', (data)=>{
    socket.userName = data.userName;                      // 새로운 채팅 사용자의 nickname을 socket.username에 저장
    console.log('joinNewUser 이벤트 발생 : ', data);      // 클라이언트로부터 받은 데이터
    io.emit('joinNewUser', {userName: data.userName})     // 모든 클라이언트에게 메시지 전송

    userName_list.push(data.userName);
    console.log('userName_list: ', userName_list);        // 사용자 이름 리스트 출력
    
    // 접속 사용자 숫자가 gameStrartingUserCount이면 클라이언트에게 'playGame' 이벤트 전송
    if(userName_list.length == gameStrartingUserCount){
      io.emit('playGame');
    }
  })

  // 새로운 채팅 메시지가 수신되면 모두에게 전송
  socket.on('newMsg', (data)=>{
    console.log('newMsg 이벤트 발생 : ', data);      // 클라이언트로부터 받은 데이터
    io.emit('broadcastMsg', {msg: data.msg, userName: socket.userName})     // 모든 클라이언트에게 메시지 전송
  })

  // 퀴즈 요청
  socket.on('requestQuiz', function(){
    console.log('requestQuiz 요청 이벤트 발생 : ');
    requestCount++;

    if(requestCount == gameStrartingUserCount){
      // 퀴즈 리스트에서 랜덤하게 퀴즈를 선택한다.
      var randomNum = Math.floor(Math.random() * quizList.length);
      var selectedQuiz = quizList[randomNum];
      dap = selectedQuiz.dap;  // 정답 저장
      quizList.splice(randomNum, 1)  // 이미 출제된 퀴즈 제거
      console.log(selectedQuiz)
      requestCount=0;
      io.emit('responseQuiz', {quiz: selectedQuiz.quiz});
    }
  })

  // 클라이언트 접속 해지 이벤트
  socket.on('disconnect', (reason)=>{
    console.log(`${socket.userName} 접속 해제`)
    console.log('reason: ', reason);

    userName_list.pop(socket.userName);
    console.log('userName_list: ', userName_list);                // 사용자 이름 리스트 출력

    io.emit('disconnetUser', {userName: socket.userName})
  })
});