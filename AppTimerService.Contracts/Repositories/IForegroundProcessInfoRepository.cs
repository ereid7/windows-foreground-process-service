﻿using AppTimerService.Contracts.Models;

namespace AppTimerService.Contracts.Repositories
{
    public interface IForegroundProcessInfoRepository
    {
        IForegroundProcessInfoEntity GetById(int id);
        void AddItem(IForegroundProcessInfoEntity entity);
        void UpdateItem(IForegroundProcessInfoEntity entity);
        void SaveChanges();
    }
}