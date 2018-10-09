#include "Heap.h"

Heap::Heap(int size)
{
	// creates heap with the current size
	this->size = size;
	arr = new int[size];
	indexCount = 0;
}

int Heap::RemoveFirst()
{
	int currentNum = arr[0]; // gets the first element
	arr[0] = arr[--indexCount]; // makes the lust element - first
	SortDown(); // heap's down sort
	return currentNum;
}

void Heap::Add(int num)
{
	indexCount++;
	arr[indexCount - 1] = num; // adds new element in the end of the heap
	SortUp(); // heap's up sort
}

void Heap::SortUp()
{
	int currentIndex = indexCount - 1; // makes "currentIndex" equal to the index of the last heap element
	while (true)
	{
		// if parent's value is greater
		if (arr[GetParent(currentIndex)] > arr[currentIndex])
		{
			// swaps parent's and child's values
			Swap(GetParent(currentIndex), currentIndex);
			// makes "currentIndex" equal to the index of the parent element
			currentIndex = GetParent(currentIndex);
		}
		else
		{
			// returns if child's value is greater
			return;
		}
	}
}

// swaps two values
void Heap::Swap(int indexA, int indexB)
{
	int midle = arr[indexA];
	arr[indexA] = arr[indexB];
	arr[indexB] = midle;
}

void Heap::SortDown()
{
	int currentIndex = 0; // makes "currentIndex" equal to the index of the first heap element
	while (true)
	{
		int swapIndex = GetLeftChild(currentIndex); // makes "swapIndex" equal to the index of the lieft child element
		// if value of the left child is greater than value of the right child:
		// makes "swapIndex" equal to the index of the right child element
		if (arr[swapIndex] > arr[GetRightChild(currentIndex)])
		{
			swapIndex = GetRightChild(currentIndex);
		}
		// if parent's value is less/equal than/to child's value - break
		if (arr[currentIndex] < arr[swapIndex] || arr[currentIndex] == arr[swapIndex])
		{
			break;
		}
		// swaps parent's and child's value
		Swap(swapIndex, currentIndex);
		// makes "currentIndex" equal to the child index
		currentIndex = swapIndex;
	}
}

Heap::~Heap()
{
	delete[] arr;
}
