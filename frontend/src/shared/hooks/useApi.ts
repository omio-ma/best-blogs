import { useQuery, useMutation, UseQueryOptions, UseMutationOptions } from '@tanstack/react-query';
import apiClient, { ApiResponse } from '../api/apiClient';
import { AxiosError } from 'axios';

interface ApiError {
  message: string;
  errors?: string[];
  status?: number;
}

// Generic hook for GET requests
export function useApiQuery<T>(
  key: string | string[],
  url: string,
  options?: Omit<UseQueryOptions<ApiResponse<T>, ApiError>, 'queryKey' | 'queryFn'>
) {
  return useQuery<ApiResponse<T>, ApiError>({
    queryKey: Array.isArray(key) ? key : [key],
    queryFn: async () => {
      const response = await apiClient.get<ApiResponse<T>>(url);
      return response.data;
    },
    ...options,
  });
}

// Generic hook for POST/PUT/DELETE requests
export function useApiMutation<TData, TVariables>(
  method: 'post' | 'put' | 'delete',
  url: string | ((variables: TVariables) => string),
  options?: UseMutationOptions<ApiResponse<TData>, ApiError, TVariables>
) {
  return useMutation<ApiResponse<TData>, ApiError, TVariables>({
    mutationFn: async (variables: TVariables) => {
      const endpoint = typeof url === 'function' ? url(variables) : url;
      const response = await apiClient[method]<ApiResponse<TData>>(endpoint, variables);
      return response.data;
    },
    ...options,
  });
}
