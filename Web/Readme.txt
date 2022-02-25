Net
	.Data
		.Classes : [DBFirst]방식에서 DTO로 쓰일 클래스들. Database프로젝트에서 가져오면 편함(Key 지정은 해야함).
		.DataModels : [CodeFirst]방식에서 마이그레이션 및 DTO로 쓰일 클래스들.
		.ViewModels : View용 DTO
	.Database : refactoring용 프로젝트(core가 아닌 .netframework 클래스 라이브러리 프로젝트로 생성)
		.Data : 클래스를 'ADO.NET 엔터티 데이터 모델'로 추가 > 중요한 연결 문자열 포함 선택(비밀번호를 포함 시) > 생성된 개체의 이름을 복수화 또는 단수화 체크 > tt파일 안에 cs파일에 테이블에 대한 정보가 들어있음
	.Service
		.Config : 설정파일
		.Data : Migration용 Context
		.Interfaces : 인터페이스
		.Migrations : 자동으로 생성되는 마이그레이션 파일
		.Services : 서비스
	.Web
		.Controller : 컨트롤러
		.Views : 뷰
		.DataProtector : 암호화에 필요한 키 값을 생성해서 저장하는 폴더 (다른데 위치해도 됨)
		.Extensions : 확장매서드가 담긴 폴더. Newtonsoft.Json 패키지 설치

로깅 패키지는 Net6를 지원하는 NLog로 적용
(https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6)
1. 패키지 설치: Nlog, Nlog.Web.AspNetCore
2. root디렉토리에 nlog.config 추가 : 저장위치 등 옵션 지정 가능
3. program.cs 수정 (logger 사용)
4. appsettings.json, appsettings.Development.json 수정
5. 사용하고자 하는 controller에서 ILogger<Controller클래스명> 의존성 주입하여 사용 -> HomeController에 적용

//vs 단축키
- ctrl+. : import
- ctrl+space : properties
- ctrl+z : 코드 재정렬