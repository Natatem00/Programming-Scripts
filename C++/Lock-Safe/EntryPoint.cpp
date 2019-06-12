#include "src\DataStructures\LockFreeQueue.h"
#include "src\DataStructures\LockFreeStack.h"
#include <iostream>
int main()
{

	//LockFreeQueue<int> que = LockFreeQueue<int>(2);
	//
	//que.Add(11);
	//que.Add(-2);
	//que.Add(6);
	//
	//std::cout << que.Remove() << std::endl;
	//std::cout << que.Remove() << std::endl;

	LockFreeStack<int> st;
	st.Push(2);
	st.Push(3);
	st.Push(11);

	std::cout << st.Pop() << std::endl;
	std::cout << st.Pop() << std::endl;
	return 0;
}