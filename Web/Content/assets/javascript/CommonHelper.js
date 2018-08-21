var CommonHelper = function ($rootScope, $localstorage, $timeout, $q, $http) {
    var service = {};

    service.ConfigUrl = "Configs/"; 
    service.MenuUrl = "Menus/"; 
    service.CurrencyUrl = "Currencies/"; 
    service.TransactionUrl = "Transactions/"; 
    service.RoleUrl = "Roles/"; 
    service.UserUrl = "Users/"; 

    service.DepWithType = {};
    service.DepWithType.Deposit = 0;
    service.DepWithType.Withdraw = 1;

    service.StatusTransaction = {};
    service.StatusTransaction.Pending = 0;
    service.StatusTransaction.Confirmed = 1;
    service.StatusTransaction.Cancel = 2;
    
    return service;
}
CommonHelper.$inject = ["$rootScope", "$localstorage", "$timeout", "$q", "$http"];
