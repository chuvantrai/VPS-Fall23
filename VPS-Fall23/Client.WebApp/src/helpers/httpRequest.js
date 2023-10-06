/* eslint-disable no-undef */
import axios from 'axios';

const request = axios.create({
  baseURL: process.env.VITE_API_GATEWAY,
});

export default request;
