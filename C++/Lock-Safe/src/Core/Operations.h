#pragma once
#include <stdint.h>

template<typename T>
bool CAS(T* ptr, T oldVal, T newVal)
{
	if (*ptr == oldVal)
	{
		*ptr = newVal;
		return true;
	}
	return false;
}

template<typename T, typename G>
bool CAS2(T* ptr, T oldValue, T newValue, G* secondPtr, G secondOldValue, G secondNewValue)
{
	if (*ptr == oldValue && *secondPtr == secondOldValue)
	{
		*ptr = newValue;
		*secondPtr = secondNewValue;
		return true;
	}
	return false;
}
