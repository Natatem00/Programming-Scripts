#pragma once
#include "..\DataStructures\Node.h"
#include <stdint.h>
#include "..\Core\Operations.h"


template<typename T>
class LockFreeQueue
{
public:

	void Add(T node);
	T Remove();

	LockFreeQueue(T nodeVal)
	{
		head = tail = new Node<T>(nodeVal);
	}

	void Clear();

	~LockFreeQueue()
	{
		Clear();
	}

private:

	Node<T>* volatile head;
	uint32_t volatile pops;
	Node<T>* volatile tail;
    uint32_t volatile pushes;
};

template<typename T>
inline void LockFreeQueue<T>::Add(T nodeVal)
{
	uint32_t tempPushes;
	Node<T>* tempTail;

	Node<T>* newNode = new Node<T>(nodeVal);

	while (true)
	{
		tempPushes = pushes;
		tempTail = tail;

		if (CAS<Node<T>* volatile>(&tempTail->nextNode, reinterpret_cast<Node<T>*>(nullptr), newNode))
		{
			break;
		}
		else
		{
			CAS2<Node<T>* volatile, uint32_t volatile>(&tail, tempTail, tail->nextNode, &pushes, tempPushes, pushes + 1);
		}
	}

	CAS2<Node<T>* volatile, uint32_t volatile>(&tail, tempTail, newNode, &pushes, tempPushes, pushes + 1);
}

template<typename T>
inline T LockFreeQueue<T>::Remove()
{
	Node<T>* tempHead;
	T value = T();
	while (true)
	{
		uint32_t tempPops = pops;
		uint32_t tempPushes = pushes;
		tempHead = head;
		Node<T>* next = tempHead->nextNode;

		if (tempPops != pops)
		{
			continue;
		}

		if (tempHead == tail)
		{
			if (next == nullptr)
			{
				tempHead = nullptr;
				value = NULL;
				break;
			}

			CAS2<Node<T>* volatile, uint32_t volatile>(&tail, tempHead, next, &pushes, tempPushes, pushes + 1);
		}
		else if (next != nullptr)
		{
			if (CAS2<Node<T>* volatile, uint32_t volatile>(&head, tempHead, next, &pops, tempPops, pops + 1))
			{
				value = tempHead->value;
				delete tempHead;
				break;
			}
		}
	}
	return value;
}

template<typename T>
inline void LockFreeQueue<T>::Clear()
{
	while (Remove() != NULL);
}
