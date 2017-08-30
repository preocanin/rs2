(function () {

    angular
        .module('app')
        .controller('clientsCtrl', clientsCtrl);

    clientsCtrl.$inject = ['UserService', '$location', '$rootScope', 'FlashService', 'toastr'];
    function clientsCtrl(UserService, $location, $rootScope, FlashService, toastr) {
        "use strict";

        var vm = this;
        vm.data = {};  //objekat ima counts i users
        vm.obradiKolonu = obradiKOlonu;

        var columnDefs = [
            { headerName: "ID", field: "userId", cellRenderer: vm.obradiKolonu },
            { headerName: "Username", field: "username" }
        ];

        vm.gridOptions = {
            enableColResize: true,
            debug: true,
            rowSelection: 'multiple',
            rowDeselection: true,
            columnDefs: columnDefs,
            // tell grid we want virtual row model type
            rowModelType: 'infinite',
            // how big each page in our page cache will be, default is 100
            paginationPageSize: 100,
            // how many extra blank rows to display to the user at the end of the dataset,
            // which sets the vertical scroll and then allows the grid to request viewing more rows of data.
            // default is 1, ie show 1 row.
            cacheOverflowSize: 2,
            // how many server side requests to send at a time. if user is scrolling lots, then the requests
            // are throttled down
            maxConcurrentDatasourceRequests: 2,
            // how many rows to initially show in the grid. having 1 shows a blank row, so it looks like
            // the grid is loading from the users perspective (as we have a spinner in the first col)
            infiniteInitialRowCount: 1,
            // how many pages to store in cache. default is undefined, which allows an infinite sized cache,
            // pages are never purged. this should be set for large data to stop your browser from getting
            // full of data
            maxBlocksInCache: 2,
            onGridReady: function () {
                setRowData(vm.data);
            }
        };

        function setRowData() {
            var dataSource = {
                rowCount: null, // behave as infinite scroll
                getRows: function (params) {
                    console.log('asking for ' + params.startRow + ' to ' + params.endRow);
                    // At this point in your code, you would call the server, using $http if in AngularJS 1.x.
                    // To make the demo look real, wait for 500ms before returning
                    UserService.GetAll(params.startRow, 100)
                        .then(function (response) {
                            vm.count = response.count;
                            vm.data = response.users;

                            var rowsThisPage = vm.data.slice(params.startRow, params.endRow);
                            // if on or after the last page, work out the last row.
                            var lastRow = -1;
                            if (vm.count <= params.endRow) {
                                lastRow = vm.data.length;
                            }
                            // call the success callback
                            params.successCallback(rowsThisPage, lastRow);
                        });
                }
            };

            vm.gridOptions.api.setDatasource(dataSource);
            vm.gridOptions.api.sizeColumnsToFit();

        }

        function obradiKOlonu(params) {
            return '<a href="#!/clients/' + params.value + '">' + params.value + '</a>';
        };

        function uzmiId(rows) {
            var niz = [];
            rows.forEach(function (element) {
                niz.push(element.userId);
            });
            return niz;
        }

        vm.obrisi = function () {
            var rows = vm.gridOptions.api.getSelectedRows();
            rows = uzmiId(rows);
            UserService.Delete(rows)
                .then(function (response) {
                    console.log(response);
                    toastr.success('Uspesno brisanje');
                    setRowData();
                });
        }

    }

})();
