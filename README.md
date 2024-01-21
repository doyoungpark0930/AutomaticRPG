오브젝트풀링 사용. => 다중오브젝트풀링(한번 클릭 시 탐색을 해야하므로 List,Queue,Stack보다는 dictionary사용)
Ui에서 필요한 것들. 가령 A-1,A-2,A-3,B-1,B-2,B-3,C-1,C-2,C-3,C-4가 있다. 이 때 C-1~C-4는 세트라 한번에 호출해야하는데
오브젝트 풀링에서 한번에 다루면 코드가 길어지니 C인스턴스를 따로 만들어관리.
에를들어 TerritoryManagementExitButton과 BulidingGround는 한 세트인데, MainUi에서 불러내기엔 코드가 복잡해지니, TerritoryManagement라는 컴포넌트를 이용해 관리한다. 
TerritoryManagemnt또한 반복 호출되므로 인스턴스화 하여 오브젝트 풀에 넣는다.

코루틴도 가비지컬렉터에 부하를 줄 수 있으니 나중에 코드 리팩토링에 반영하기
