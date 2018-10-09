#pragma once
#include <stdio.h>
class Heap
{
public:
	Heap(int size);

	int RemoveFirst();

	void Add(int num);

	~Heap();
private:
	int indexCount;
	int size;
	int* arr;

	void SortUp();

	int GetParent(int index) { return (index - 1) / 2 > 0 ? (index - 1) / 2 : 0; }
	int GetLeftChild(int index) { return (index * 2) + 1 < indexCount ? (index * 2) + 1 : indexCount - 1; }
	int GetRightChild(int index) { return (index * 2) + 2 < indexCount ? (index * 2) + 2 : indexCount - 1; }

	void Swap(int indexA, int indexB);
	void SortDown();
};
