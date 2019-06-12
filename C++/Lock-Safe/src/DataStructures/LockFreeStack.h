#pragma once
#include "..\Core\Operations.h"
#include "..\DataStructures\Node.h"

template<typename T>
class LockFreeStack
{
public:

	LockFreeStack() : head(nullptr), pops(0) {};

	void Push(T node);
	T Pop();
	void Clear();

	~LockFreeStack()
	{
		Clear();
	}
private:

	Node<T>* volatile head;
	uint32_t volatile pops;
};

template<typename T>
void LockFreeStack<T>::Push(T nodeValue)
{
	Node<T>* newNode = new Node<T>(nodeValue);
	while (true)
	{
		newNode->nextNode = head;
		if (CAS<Node<T>* volatile>(&head, newNode->nextNode, newNode))
		{
			break;
		}
	}
}

template<typename T>
T LockFreeStack<T>::Pop()
{
	while (true)
	{
		Node<T>* tempHead = head;
		uint32_t tempPops = pops;

		if (tempHead == nullptr)
		{
			return NULL;
		}

		Node<T>* next = tempHead->nextNode;
		if (CAS2<Node<T>* volatile, uint32_t volatile>(&head, tempHead, next, &pops, tempPops, pops + 1))
		{
			T value = tempHead->value;
			delete tempHead;
			return value;
		}
	}
}

template<typename T>
inline void LockFreeStack<T>::Clear()
{
	while (Pop() != NULL);
}
