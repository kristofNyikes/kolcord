const ErrorModal = ({
  error: errorResponse,
  errorList,
}: {
  error: Response | null;
  errorList: string[];
}) => {
  return (
    <div className="absolute top-0 right-0 m-2 w-96 min-h-20 rounded-md bg-red-600 p-3">
      {errorResponse && (
        <span>
          Error: {errorResponse.status} {errorResponse.statusText}
        </span>
      )}
      <ul className="ml-6">
        {errorList.length > 0 &&
          errorList.map((e, i) => {
            return (
              <li key={i} className="list-disc">
                {e}
              </li>
            );
          })}
      </ul>
    </div>
  );
};

export default ErrorModal;
