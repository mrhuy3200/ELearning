    var topicFlag = 0;
    $scope.CreateTopic = function () {
        var TopicDTO = {
            Name: $scope.TopicName
        };
        if ($scope.TopicName != null && $scope.TopicName != "" && topicFlag == 0) {
            $http({
                method: 'POST',
                url: '/Lop/CreateTopic',
                data: JSON.stringify(TopicDTO)
            }).then(function successCallback(response) {
                if (response.data != 0) {
                    TopicDTO["ID"] = response.data;
                    console.log(TopicDTO);
                    $scope.Topics.push(TopicDTO);
                    $scope.TopicName = "";
                }
                else {
                    alert("Fail");
                }
            });
        }

        if ($scope.TopicName != null && $scope.TopicName != "" && topicFlag != 0) {
            EditTopic(topicFlag);
        }
    }
    $scope.SelectTopic = function (Index) {
        document.getElementById("TopicInput").value = $scope.Topics[Index].Name;
        topicFlag = Index;
    }
    function EditTopic(Index) {
        var TopicDTO = $scope.Topics[Index];
        TopicDTO.Name = $scope.TopicName;
        $http({
            method: 'POST',
            url: '/Lop/EditTopic',
            data: JSON.stringify(TopicDTO)
        }).then(function successCallback(response) {
            if (response.data == 1) {
                $scope.Topics[Index].Name = $scope.TopicName;
                $scope.TopicName = "";
            }
            else {
                alert("Fail");
            }
        });

    }
    $scope.DeleteTopic = function (Index) {
        console.log($scope.Topics[Index]);
        $http({
            method: 'POST',
            url: '/Lop/DeleteTopic',
            data: JSON.stringify($scope.Topics[Index])
        }).then(function successCallback(response) {
            if (response.data == 1) {
                $scope.Topics.splice(Index,1);
            }
            else {
                alert("Fail");
            }
        });

    }