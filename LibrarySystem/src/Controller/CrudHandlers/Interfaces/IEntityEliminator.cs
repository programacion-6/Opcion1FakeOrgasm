﻿namespace LibrarySystem;

public interface IEntityEliminator<T, R> where T : EntityBase
{
    Task TryToDeleteEntity();
}
