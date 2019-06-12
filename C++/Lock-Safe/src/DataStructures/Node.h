#pragma once

template<typename T>
struct Node
{
	T value;
	Node<T>* volatile nextNode;

	Node() : value(), nextNode(nullptr) {};
	Node(T value) : value(value), nextNode(nullptr) {};
};