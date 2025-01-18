import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    vus: 3,
    duration: '30s',
};

export default function () {

    //GetRoute endpoint
    const res = http.get('http://localhost:8080/Route?RouteId=88');
    check(res, {
        'status was 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    }, {endpoint: 'GetRoute'});
    sleep(1);

    //GetMaxSpeed endpoint
    const res2 = http.get('http://localhost:8080/Route/MaxSpeed?Routeid=88');
    check(res2, {
        'status was 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    }, {endpoint: 'GetMaxSpeed'});
    sleep(1);

    //GetMaxG endpoint
    const res3 = http.get('http://localhost:8080/Route/MaxG?Routeid=88');
    check(res3, {
        'status was 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    }, {endpoint: 'GetMaxG'});
    sleep(1);

    //GetMaxLeanAngle endpoint
    const res4 = http.get('http://localhost:8080/Route/MaxLean?Routeid=88');
    check(res4, {
        'status was 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    }, {endpoint: 'GetMaxLeanAngle'});
    sleep(1);

    //Login endpoint
    const res5 = http.get('http://localhost:8080/Login?identifier=testlogin&password=testlogin');
    check(res5, {
        'status was 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    }, {endpoint: 'Login'});
    sleep(1);
}
