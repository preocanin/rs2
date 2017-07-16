(function () {
    'use strict';
 
    angular
        .module('app')
        .factory('RecordService', RecordService);
 
    RecordService.$inject = ['$http'];
    function RecordService($http) {
        var service = {};
 
        service.GetAll = GetAll;
        service.AddRecord = AddRecord;
        service.AddRecordsFromFile = AddRecordsFromFile;
        service.GetRecordsFromFile = GetRecordsFromFile;
        service.UpdateRecord = UpdateRecord;
        service.DeleteRecordsByUser = DeleteRecordsByUser;
        service.DeleteAllRecords = DeleteAllRecords;
 
        return service;
 
        function GetAll(offset) {
            return $http.get('http://localhost:5000/api/records?offset=' + offset).then(handleSuccess, handleError('Error getting all records'));
        }
 
        function AddRecord(record) {
            return $http.post('http://localhost:5000/api/records', record).then(handleSuccess, handleError('Error adding new record'));
        }
 
        function AddRecordsFromFile(file) {
            return $http.post('/api/records/file', file).then(handleSuccess, handleError('Error adding records from file'));
        }
 
        function GetRecordsFromFile() {
            return $http.get('http://localhost:5000/api/records/file').then(handleSuccess, handleError('Error getting records from file'));
        }
 
        function UpdateRecord(userId, records) {
            return $http.put('http://localhost:5000/api/records/' + userId, records).then(handleSuccess, handleError('Error updating records'));
        }
 
        function DeleteRecordsByUser(userId, records) {
            return $http.delete('http://localhost:5000/api/records/' + userId, records).then(handleSuccess, handleError('Error deleting records by user'));
        }

        function DeleteAllRecords() {
            return $http.delete('http://localhost:5000/api/records/all').then(handleSuccess, handleError('Error deleting all records'));
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