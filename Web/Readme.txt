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

//vs 단축키
- ctrl+. : import
- ctrl+space : properties