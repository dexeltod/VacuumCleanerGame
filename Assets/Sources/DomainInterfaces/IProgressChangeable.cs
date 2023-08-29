using System;

public interface IProgressChangeable
{
	event Action Changed;
}