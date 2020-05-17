var EditLessionApp = angular.module("EditLessionApp", []);

EditLessionApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])

EditLessionApp.controller('EditLessionController', function ($scope, $http, $window, $sce) {
    var formdata = new FormData();
    var file;
    $scope.InitLessionID = function (id) {
        $scope.LessionID = id;
        LoadLession();
    }

    function LoadLession() {
        $http({
            method: 'GET',
            url: '/Lop/GetLessionByID/' + $scope.LessionID
        }).then(function successCallback(response) {
            console.log(response.data);
            $scope.Lession = response.data;
            CKEDITOR.instances.Content.setData($scope.Lession.Content);
            $scope.URL = $sce.trustAsResourceUrl($scope.Lession.URL);
        });
    }
    $scope.getTheFiles = function ($files) {
        console.log($files)
        file = $files[0];
        console.log(formdata);
    };
    $scope.save = function () {
        //var a = angular.element("#Content").val();
        var url = $scope.Lession.URL;
        $scope.Lession.URL = url.replace("watch?v=", "embed/");
        $scope.Lession.Content = CKEDITOR.instances.Content.getData();
        console.log($scope.Lession);

        $http({
            method: 'POST',
            url: '/Lop/EditLession',
            data: JSON.stringify($scope.Lession)
        }).then(function successCallback(response) {
            if (file != null) {
                ID = $scope.Lession.ID;
                console.log(ID);
                console.log('filename ' + file.name);
                var blob = file.slice(0, file.size, 'image/jpg');
                newFile = new File([blob], ID + '.jpg', { type: 'image/jpg' });
                console.log('filename ' + newFile);
                formdata.set(0, newFile);
                console.log(formdata.get(0));
                $http({
                    method: 'POST',
                    url: '/api/API/UploadLessionImage',
                    data: formdata,
                    headers: {
                        'Content-Type': undefined
                    }
                }).then(function successCallback(response) {
                    alert(response.data);
                    if (response.data == "OK") {
                        $window.location.href = '/Lop/LessionDetail/' + ID;
                    }
                });

            }
            else {
                alert(response.data)
                if (response.data == "OK") {
                    $window.location.href = '/Lop/LessionDetail/' + $scope.Lession.ID;
                }
            }
        });
    }
});
