(function () {
    'use strict';
 
    angular
        .module('app')
        .factory('UserService', UserService);
 
    UserService.$inject = ['$http'];
    function UserService($http) {
        var service = {};
 
        service.GetAll = GetAll;
        service.GetById = GetById;
//        service.GetByUsername = GetByUsername;
        service.Create = Create;
        service.Update = Update;
        service.Delete = Delete;
 
        return service;
 
        function GetAll(offset, limit) {
            return $http.get('http://localhost:5000/api/users?offset=' + offset + '&limit=' + limit).then(handleSuccess, handleError('Error getting all users'));
        }
 
        function GetById(id) {
            return $http.get('http://localhost:5000/api/users/' + id).then(handleSuccess, handleError('Error getting user by id'));
        }
 
//        function GetByUsername(username) {
//            return $http.get('/api/users/' + username).then(handleSuccess, handleError('Error getting user by username'));
//        }
 
        function Create(user) {
            return $http.post('http://localhost:5000/api/users', user).then(handleSuccess, handleError('Error creating user'));
        }
 
        function Update(user) {
            return $http.put('http://localhost:5000/api/users/' + user.id, user).then(handleSuccess, handleError('Error updating user'));
        }
 
        function Delete(id) {
            return $http.delete('http://localhost:5000/api/users/' + id).then(handleSuccess, handleError('Error deleting user'));
        }
 
        // private functions
 
        function handleSuccess(res) {
            return res.data;
        }
 
        function handleError(error) {
            return function () {
                return { success: false, message: error };
            };
        }
    }
 
})();